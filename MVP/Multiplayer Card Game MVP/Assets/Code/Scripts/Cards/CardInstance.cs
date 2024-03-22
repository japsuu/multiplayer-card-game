using System;
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
        
        private PlayerHandManager _playerHandManager;
        private Vector3 _homePosition;
        private bool _isBeingDragged;
        private CellHighlightGroup _highlighter;
        
        public CardData Data { get; private set; }


        public void Initialize(CardData data, PlayerHandManager playerHandManager)
        {
            Data = data;
            _playerHandManager = playerHandManager;
            
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
            if (Data.PlayTarget == CardPlayTarget.Cell)
                TargetingArrow.Instance.Activate(transform);

            _highlighter = BoardManager.Instance.CreateHighlightGroup();
            Data.BuildHighlighter(_highlighter);
            
            _isBeingDragged = true;
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (!_isBeingDragged)
                return;

            if (TargetingArrow.Instance.TryGetCell(out Vector2Int cell))
            {
                BoardManager.Instance.TryGetCellToWorld(cell, out Vector3 pos);
                _highlighter.transform.position = pos;
                _highlighter.gameObject.SetActive(true);
            }
            else
            {
                _highlighter.gameObject.SetActive(false);
            }
            Data.UpdateHighlighter(_highlighter, cell);
            
            if (Data.PlayTarget == CardPlayTarget.Anywhere)
                transform.position = eventData.position;
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            DestroyHighlighter();
            
            if (!_isBeingDragged)
                return;
            
            switch (Data.PlayTarget)
            {
                case CardPlayTarget.Cell:
                {
                    if (TargetingArrow.Instance.TryGetCell(out Vector2Int cell))
                        _playerHandManager.OnCardPlayed(this, cell);
                
                    TargetingArrow.Instance.Deactivate();
                    break;
                }
                case CardPlayTarget.Anywhere:
                    _playerHandManager.OnCardPlayed(this, Vector2Int.zero);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _isBeingDragged = false;
        }


        private void Update()
        {
            if (_isBeingDragged)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _isBeingDragged = false;

                    if (Data.PlayTarget == CardPlayTarget.Cell)
                        TargetingArrow.Instance.Deactivate();
                    DestroyHighlighter();
                }
                return;
            }
            
            MoveTowardsHome();
        }


        private void DestroyHighlighter()
        {
            if (_highlighter == null)
                return;
            
            Destroy(_highlighter.gameObject);
            _highlighter = null;
        }


        private void MoveTowardsHome()
        {
            transform.position = Vector3.Lerp(transform.position, _homePosition, Time.deltaTime * 10f);
        }
    }
}