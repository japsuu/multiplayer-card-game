using System;
using System.Collections.Generic;
using Cards.Data;
using UnityEngine;

namespace Cards
{
    public class PlayerHandManager : MonoBehaviour
    {
        [Header("General")]
        
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
        
        private readonly List<Transform> _cardPosTargets = new();
        private readonly PlayerHand _hand = new();
        
        
        private void Start()
        {
            foreach (CardData cardData in _startingCards)
                AddCardToHand(cardData);
        }


        public void AddCardToHand(CardData cardData)
        {
            CardInstance card = Instantiate(_cardPrefab, _cardRoot);
            card.Initialize(cardData, this);
            _hand.Cards.Add(card);
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
            
            CardData randomCard = _spawnableRandomCards[UnityEngine.Random.Range(0, _spawnableRandomCards.Count)];
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


        public void OnCardPlayed(CardInstance cardInstance, Vector2Int cell)
        {
            StartCoroutine(cardInstance.Data.OnPlay(cell));
            
            _hand.Cards.Remove(cardInstance);
            Destroy(cardInstance.gameObject);
        }
    }
}