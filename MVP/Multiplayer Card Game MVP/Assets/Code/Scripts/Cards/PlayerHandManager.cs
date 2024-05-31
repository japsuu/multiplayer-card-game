using System.Collections;
using System.Collections.Generic;
using Cards.Data;
using Singletons;
using UnityEngine;

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
        
        public int CardsPlayedThisTurn { get; private set; }
        public int CardCount => _hand.Cards.Count;
        public int DrawLimit => _drawLimit;
        
        
        public CardData RemoveCardFromDrawPile()
        {
            return _spawnableRandomCards[Random.Range(0, _spawnableRandomCards.Count)];
        }


        public void AddCardToHand(CardData cardData)
        {
            CardInstance card = Instantiate(_cardPrefab, _cardRoot);
            card.Initialize(cardData);
            _hand.Cards.Add(card);
        }


        public void PlayCard(CardInstance cardInstance, IEnumerator playCoroutine)
        {
            CardsPlayedThisTurn++;
            
            StartCoroutine(playCoroutine);
            
            _hand.Cards.Remove(cardInstance);
            Destroy(cardInstance.gameObject);
        }


        public void ShowHand()
        {
            CardsPlayedThisTurn = 0;
            _cardsPanel.position = _cardsPanelOriginalPos;
#warning TODO: Show tabled cards
        }


        public void HideHand()
        {
            CardsPlayedThisTurn = 0;
            _cardsPanel.position = _cardsPanelOriginalPos - Vector3.up * _cardsPanelHideOffset;
#warning TODO: Hide tabled cards
        }
        
        
        private void Start()
        {
            _cardsPanelOriginalPos = _cardsPanel.position;
            
            foreach (CardData cardData in _startingCards)
                AddCardToHand(cardData);
            
            HideHand();
        }


        private void Update()
        {
            if (_cardPosTargets.Count != _hand.Cards.Count)
                UpdateCardPosTargets();
            
            for (int i = 0; i < _hand.Cards.Count; i++)
            {
                CardInstance card = _hand.Cards[i];
                card.UpdateHomePosition(GetCardPosition(i));
            }
            
            SpawnRandomDebugCard();
        }


        private void SpawnRandomDebugCard()
        {
            if (!Input.GetKeyDown(KeyCode.P))
                return;
            
            CardData randomCard = _spawnableRandomCards[Random.Range(0, _spawnableRandomCards.Count)];
            AddCardToHand(randomCard);
            Debug.Log($"Added random card '{randomCard.CardName}' to hand.");
        }


        private Vector3 GetCardPosition(int i)
        {
            return _cardPosTargets[i].position;
        }


        private void UpdateCardPosTargets()
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
    }
}