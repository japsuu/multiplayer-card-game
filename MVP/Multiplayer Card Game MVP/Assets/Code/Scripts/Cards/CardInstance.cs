using Boards;
using Cards.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{
    /// <summary>
    /// In-world representation of a card.
    /// Instantiated to the world by <see cref="PlayerHandManager"/>.
    /// </summary>
    public class CardInstance : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private TMP_Text _nameText;
        
        [SerializeField]
        private TMP_Text _manaCostText;
        
        [SerializeField]
        private TMP_Text _descriptionText;
        
        [SerializeField]
        private Image _artImage;
        
        private Vector3 _homePosition;
        private bool _isBeingDragged;
        
        [HideInInspector] public CellHighlightGroup Highlighter;
        
        public CardData Data { get; private set; }


        public void Initialize(CardData data)
        {
            Data = data;
            
            _nameText.text = data.CardName;
            _manaCostText.text = data.ManaCost.ToString();
            _descriptionText.text = data.Description;
            _artImage.sprite = data.Sprite;
        }
        
        
        public void UpdateHomePosition(Vector3 homePosition)
        {
            _homePosition = homePosition;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            Data.OnStartDrag(this);
            
            _isBeingDragged = true;
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (!_isBeingDragged)
                return;
            
            Data.OnDrag(this);
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            DestroyHighlighter();
            
            if (!_isBeingDragged)
                return;
            
            Data.OnEndDrag(this, true);
            _isBeingDragged = false;
        }


        private void Update()
        {
            if (_isBeingDragged)
            {
                if (!Input.GetMouseButtonDown(1))
                    return;
            
                Data.OnEndDrag(this, false);
                _isBeingDragged = false;

                DestroyHighlighter();
                return;
            }
            
            MoveTowardsHome();
        }


        private void DestroyHighlighter()
        {
            if (Highlighter == null)
                return;
            
            Destroy(Highlighter.gameObject);
            Highlighter = null;
        }


        private void MoveTowardsHome()
        {
            transform.position = Vector3.Lerp(transform.position, _homePosition, Time.deltaTime * 10f);
        }
    }
}