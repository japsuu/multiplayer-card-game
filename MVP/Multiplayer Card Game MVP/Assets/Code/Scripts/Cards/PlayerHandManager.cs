using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class PlayerHand
    {
        public readonly List<CardInstance> Cards;


        public PlayerHand(List<CardInstance> cards)
        {
            Cards = cards;
        }
    }
    
    public class PlayerHandManager : MonoBehaviour
    {
        [Header("General")]
        
        [SerializeField]
        private Transform _cardRoot;
        
        [SerializeField]
        private CardInstance _cardPrefab;
        
        [SerializeField]
        private List<CardData> _startingCards;
        
        [Header("Card Positioning")]

        [SerializeField]
        private GameObject _cardPosTargetPrefab;

        [SerializeField]
        private Transform _cardPosTargetRoot;
        
        private readonly List<Transform> _cardPosTargets = new();
        private PlayerHand _hand;
        
        
        private void Start()
        {
            List<CardInstance> cards = new();
            foreach (CardData cardData in _startingCards)
            {
                CardInstance card = Instantiate(_cardPrefab, _cardRoot);
                card.Initialize(cardData, this);
                cards.Add(card);
            }
            _hand = new PlayerHand(cards);
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