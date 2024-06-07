using System;
using System.Collections;
using System.Collections.Generic;
using Cards.Data;
using Entities;
using StateManagement;
using UnityEngine;
using Utils.Singletons;
using Random = UnityEngine.Random;

namespace Cards
{
    public class PlayerHandManager : SingletonBehaviour<PlayerHandManager>
    {
        [Header("General")]
        
        [SerializeField]
        private int _drawLimit = 5;
        
        [SerializeField]
        private Transform _cardRoot;
        
        [SerializeField]
        private CardInstance _cardPrefab;
        
        [SerializeField]
        private List<CardData> _startingCards;
        
        [Header("Debug")]
        
        [SerializeField]
        private List<CardData> _spawnableRandomCards;
        
        [Header("Card Positioning")]

        [SerializeField]
        private GameObject _cardPosTargetPrefab;

        [SerializeField]
        private Transform _cardPosTargetRoot;
        
        [SerializeField]
        private Transform _cardsPanel;

        [SerializeField]
        private float _cardsPanelHideOffset = 400f;
        
        private readonly List<Transform> _cardPosTargets = new();
        private readonly PlayerHand _hand = new();
        private Vector3 _cardsPanelOriginalPos;
        
        public int CardCount => _hand.Cards.Count;
        public bool CanDrawCard => CardCount < _drawLimit;
        
        
        /// <summary>
        /// Removes the top-most card from the draw pile and returns it.
        /// </summary>
        public CardData RemoveCardFromDrawPile()
        {
#warning TODO: Implement draw pile
            return _spawnableRandomCards[Random.Range(0, _spawnableRandomCards.Count)];
        }


        /// <summary>
        /// Draws a card from the draw pile and adds it to the player's hand.
        /// </summary>
        public IEnumerator DrawCard()
        {
            CardData cardData = RemoveCardFromDrawPile();
            DrawCard(cardData);
            yield return new WaitForSeconds(0.2f);
        }


        /// <summary>
        /// Adds the specified card to the player's hand.
        /// </summary>
        public void DrawCard(CardData cardData)
        {
            CardInstance card = Instantiate(_cardPrefab, _cardRoot);
            card.Initialize(cardData);
            _hand.Cards.Add(card);
            
            IEnumerator coroutine = card.Data.OnDrawn(card);
            StartCoroutine(coroutine);
        }


        /// <summary>
        /// Plays the specified card at the specified cell.
        /// </summary>
        public void PlayCard(CardInstance card, Vector2Int cell)
        {
            IEnumerator coroutine = card.Data.OnPlayed(card, cell);
            StartCoroutine(coroutine);
            
            DeactivateCard(card);

            if (!Globals.DiscardPlayedCards)
                return;
            
            if (!card.HasBeenActivated)
                throw new Exception("Card is not activated.");
                
            DiscardCard(card);
        }


        /// <summary>
        /// Activates the specified card, removing it from the player's hand.
        /// </summary>
        public void ActivateCard(CardInstance card)
        {
            IEnumerator coroutine = card.Data.OnActivated(card);
            StartCoroutine(coroutine);

            if(card.HasBeenPlayed)
                throw new Exception("The card has already been played.");
            
            card.FlagAsActivated();
            card.DestroyDiscardButton();
            
            _hand.Cards.Remove(card);
        }


        /// <summary>
        /// Deactivates the specified card, removing it from the card activation slot.
        /// </summary>
        public void DeactivateCard(CardInstance card)
        {
            card.FlagAsPlayed();
            
            IEnumerator coroutine = card.Data.OnDeactivated(card);
            StartCoroutine(coroutine);
        }
        
        
        /// <summary>
        /// Discards the specified card, removing it from the player's hand.
        /// If the card has been activated, it will be deactivated first.
        /// </summary>
        public void DiscardCard(CardInstance card)
        {
            if (card.HasBeenActivated)
                DeactivateCard(card);
            
            IEnumerator coroutine = card.Data.OnDiscarded(card);
            StartCoroutine(coroutine);
            
            _hand.Cards.Remove(card);
            Destroy(card.gameObject);
#warning TODO: Move the card to the discard pile
            /*cardInstance.transform.SetParent(CanvasManager.Instance.OverlayCanvasRoot, true);
            cardInstance.transform.localScale = Vector3.one;
            
            // Tween the card location to the bottom-right corner of the screen, then destroy it
            cardInstance.gameObject.transform.DOMove(new Vector3(Screen.width, 0, 0), 2f).OnComplete(() =>
            {
                Destroy(cardInstance.gameObject);
            });*/
        }


        public void ShowHand()
        {
            _cardsPanel.position = _cardsPanelOriginalPos;
        }


        public void HideHand()
        {
            _cardsPanel.position = _cardsPanelOriginalPos - Vector3.up * _cardsPanelHideOffset;
        }
        
        
        public void AllowDiscardCards(bool allow)
        {
#warning TODO: Move state-related stuff to the state machine
            foreach (CardInstance card in _hand.Cards)
                card.SetAllowDiscard(allow);
        }
        
        
        public void InvokeOnAttacked(BoardEntity damagingEntity, int damageAmount)
        {
            foreach (CardInstance card in CardActivationSlot.ActivatedCardInstances)
            {
                IEnumerator coroutine = card.Data.OnAttacked(card, damagingEntity, damageAmount);
                StartCoroutine(coroutine);
            }
        }


        private void InvokeOnTurnStart()
        {
            foreach (CardInstance card in CardActivationSlot.ActivatedCardInstances)
            {
                IEnumerator coroutine = card.Data.OnTurnStart(card);
                StartCoroutine(coroutine);
            }
        }


        private void InvokeOnTurnEnd()
        {
            foreach (CardInstance card in CardActivationSlot.ActivatedCardInstances)
            {
                IEnumerator coroutine = card.Data.OnTurnEnd(card);
                StartCoroutine(coroutine);
            }
        }


        private void OnEnable()
        {
            GameManager.PlayerTurnStart += InvokeOnTurnStart;
            GameManager.PlayerTurnEnd += InvokeOnTurnEnd;
        }
        
        
        private void OnDisable()
        {
            GameManager.PlayerTurnStart -= InvokeOnTurnStart;
            GameManager.PlayerTurnEnd -= InvokeOnTurnEnd;
        }
        
        
        private void Start()
        {
            _cardsPanelOriginalPos = _cardsPanel.position;
            
            foreach (CardData cardData in _startingCards)
                DrawCard(cardData);
            
            HideHand();
        }


        private void Update()
        {
            UpdateCardPositionTargets();

            SpawnRandomDebugCard();
        }


        #region Card Positioning

        private void UpdateCardPositionTargets()
        {
            if (_cardPosTargets.Count != _hand.Cards.Count)
                ReInstantiateCardPosTargets();

            for (int i = 0; i < _hand.Cards.Count; i++)
            {
                CardInstance card = _hand.Cards[i];
                card.UpdateHomePosition(GetCardTargetPosition(i));
            }
        }


        private Vector3 GetCardTargetPosition(int i)
        {
            return _cardPosTargets[i].position;
        }


        private void ReInstantiateCardPosTargets()
        {
            foreach (Transform target in _cardPosTargets)
                Destroy(target.gameObject);
            _cardPosTargets.Clear();
            
            for (int i = 0; i < _hand.Cards.Count; i++)
            {
                Transform target = Instantiate(_cardPosTargetPrefab, _cardPosTargetRoot).transform;
                _cardPosTargets.Add(target);
            }
        }

        #endregion


#if DEBUG
        private void SpawnRandomDebugCard()
        {
            if (!Input.GetKeyDown(KeyCode.P))
                return;
            
            CardData randomCard = _spawnableRandomCards[Random.Range(0, _spawnableRandomCards.Count)];
            DrawCard(randomCard);
            Debug.Log($"Added random card '{randomCard.CardName}' to hand.");
        }
#endif
    }
}