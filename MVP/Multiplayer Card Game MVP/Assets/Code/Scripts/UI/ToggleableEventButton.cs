using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class ToggleableEventButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        private RectTransform _rectTransform;
        private Vector2 _originalPosition;
        private bool _isVisible;
        
        protected abstract UnityAction ClickAction { get; }
        
        protected abstract void SubscribeToEvents(Action<bool> setVisibility);
        protected abstract void UnsubscribeFromEvents(Action<bool> setVisibility);


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalPosition = _rectTransform.anchoredPosition;
            HideSelf();
        }


        private void OnEnable()
        {
            _button.onClick.AddListener(ClickAction);
            SubscribeToEvents(SetVisibility);
        }


        private void OnDisable()
        {
            _button.onClick.RemoveListener(ClickAction);
            UnsubscribeFromEvents(SetVisibility);
        }


        private void SetVisibility(bool shouldBeVisible)
        {
            if (_isVisible == shouldBeVisible)
                return;
            
            if (shouldBeVisible)
                ShowSelf();
            else
                HideSelf();
        }
        
        
        private void ShowSelf()
        {
            _rectTransform.anchoredPosition = new Vector2(-_originalPosition.x, _originalPosition.y);
            
            // Tween to original position
            _rectTransform.DOAnchorPosX(_originalPosition.x, 0.5f);
            
            _isVisible = true;
        }
        
        
        private void HideSelf()
        {
            _rectTransform.anchoredPosition = _originalPosition;
            
            // Tween to hidden position
            _rectTransform.DOAnchorPosX(-_originalPosition.x, 0.5f);
            
            _isVisible = false;
        }
    }
}