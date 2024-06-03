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
        [SerializeField]
        private RectTransform _cardRoot;

        [SerializeField]
        private Image _discardWarningTriangle;
        
        private CardInstance _currentCard;
        private TweenerCore<Color, Color, ColorOptions> _warningTween;


        public bool CanReceiveCard(CardInstance card) => true;


        public void ReceiveCard(CardInstance card)
        {
            SetCard(card);
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


        private void Awake()
        {
            // Create a DOTween tween to flash the triangle.
            _warningTween = _discardWarningTriangle.DOFade(0.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            _warningTween.Pause();
            
            SetDiscardWarningVisibility(false);
        }


        private void SetCard(CardInstance card)
        {
            if (_currentCard != null)
                _currentCard.DeactivateAndDiscard();
            
            _currentCard = card;
            _currentCard.transform.SetParent(_cardRoot, false);
            _currentCard.transform.localPosition = Vector3.zero;
            
            PlayerHandManager.Instance.ActivateCard(card);
        }


        private void SetDiscardWarningVisibility(bool visible)
        {
            _discardWarningTriangle.gameObject.SetActive(visible);
        }
    }
}