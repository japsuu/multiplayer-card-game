using DG.Tweening;
using PhaseSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class EndTurnButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        private RectTransform _rectTransform;
        private Vector2 _originalPosition;


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalPosition = _rectTransform.anchoredPosition;
            HideSelf();
        }


        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
            GameLoopManager.PlayerTurnStart += OnPlayerTurnStart;
            GameLoopManager.PlayerTurnEnd += OnPlayerTurnEnd;
        }


        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
            GameLoopManager.PlayerTurnStart -= OnPlayerTurnStart;
            GameLoopManager.PlayerTurnEnd -= OnPlayerTurnEnd;
        }


        private static void OnButtonClicked()
        {
            GameLoopManager.RequestEndCurrentPlayerTurn();
        }


        private void OnPlayerTurnEnd()
        {
            HideSelf();
        }


        private void OnPlayerTurnStart()
        {
            ShowSelf();
        }
        
        
        private void ShowSelf()
        {
            _rectTransform.anchoredPosition = new Vector2(-_originalPosition.x, _originalPosition.y);
            
            // Tween to original position
            _rectTransform.DOAnchorPosX(_originalPosition.x, 0.5f);
            print("showing");
        }
        
        
        private void HideSelf()
        {
            _rectTransform.anchoredPosition = _originalPosition;
            
            // Tween to hidden position
            _rectTransform.DOAnchorPosX(-_originalPosition.x, 0.5f);
            print("hiding");
        }
    }
}