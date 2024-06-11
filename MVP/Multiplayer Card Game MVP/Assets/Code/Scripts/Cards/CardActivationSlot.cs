using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    /// <summary>
    /// Slot where a card can be moved to, activate the card.
    /// If there's already a card in the slot, the existing card will be discarded.
    /// </summary>
    public class CardActivationSlot : MonoBehaviour, ICardInstanceReceiver
    {
        private static int registeredActivationSlots;
        private static readonly List<CardInstance> ActivatedCards = new();
        
        [SerializeField]
        private RectTransform _cardRoot;

        [SerializeField]
        private Image _discardWarningTriangle;
        
        private CardInstance _currentCard;
        private TweenerCore<Color, Color, ColorOptions> _warningTween;
        
        public static IEnumerable<CardInstance> ActivatedCardInstances => ActivatedCards;
        public static int ActivatedCardCount => ActivatedCards.Count;


        public bool CanReceiveCard(CardInstance card) => true;


        public void ReceiveCard(CardInstance card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));
            
            if (_currentCard != null)
                RemoveOldCard();

            AssignNewCard(card);
        }


        public void OnHoverEnter(CardInstance card)
        {
            // Flash the discard warning triangle only if there's a card already in the slot.
            if (_currentCard == null || card.HasBeenActivated)
                return;
            
            SetDiscardWarningVisibility(true);
                
            if (!_warningTween.IsPlaying())
                _warningTween.Play();
        }


        public void OnHover(CardInstance cardInstance)
        {
        }


        public void OnHoverExit(CardInstance cardInstance)
        {
            SetDiscardWarningVisibility(false);
            
            if (_warningTween.IsPlaying())
                _warningTween.Pause();
        }


        private void OnEnable()
        {
            registeredActivationSlots++;
        }


        private void OnDisable()
        {
            registeredActivationSlots--;
        }


        private void Awake()
        {
            // Create a DOTween tween to flash the triangle.
            _warningTween = _discardWarningTriangle.DOFade(0.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            _warningTween.Pause();
            
            SetDiscardWarningVisibility(false);
        }


        private void RemoveOldCard()
        {
            ActivatedCards.Remove(_currentCard);
            PlayerHandManager.Instance.DiscardCard(_currentCard);
        }


        private void AssignNewCard(CardInstance card)
        {
            _currentCard = card;
            
            _currentCard.transform.SetParent(_cardRoot, false);
            _currentCard.transform.localPosition = Vector3.zero;
            
            ActivatedCards.Add(_currentCard);
            PlayerHandManager.Instance.ActivateCard(card);
            
            if (ActivatedCards.Count > registeredActivationSlots)
                throw new Exception("There are more activated cards than activation slots.");
        }


        private void SetDiscardWarningVisibility(bool visible)
        {
            _discardWarningTriangle.gameObject.SetActive(visible);
        }
    }
}