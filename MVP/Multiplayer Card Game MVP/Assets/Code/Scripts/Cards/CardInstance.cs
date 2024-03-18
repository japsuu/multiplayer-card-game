using System;
using UnityEngine;
using UnityEngine.EventSystems;
using World.Grids;

namespace Cards
{
    public class CardInstance : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public CardData Data { get; private set; }
        
        private PlayerHandManager _playerHandManager;
        private Vector3 _homePosition;
        private bool _isBeingDragged;
        private CellHighlightGroup _highlighter;


        private void Update()
        {
            if (_isBeingDragged)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _isBeingDragged = false;

                    if (Data.PlayType == CardPlayType.Cell)
                        TargetingArrow.Instance.Deactivate();
                }
                return;
            }
            
            MoveTowardsHome();
        }


        public void Initialize(CardData data, PlayerHandManager playerHandManager)
        {
            Data = data;
            _playerHandManager = playerHandManager;
        }
        
        
        public void UpdateHomePosition(Vector3 homePosition)
        {
            _homePosition = homePosition;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Data.PlayType == CardPlayType.Cell)
                TargetingArrow.Instance.Activate(transform);

            _highlighter = GridManager.Instance.CreateHighlightGroup();
            Data.BuildHighlighter(_highlighter);
            
            _isBeingDragged = true;
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (!_isBeingDragged)
                return;

            if (TargetingArrow.Instance.TryGetCell(out Vector2Int cell))
            {
                GridManager.Instance.TryGetCellToWorld(cell, out Vector3 pos);
                _highlighter.transform.position = pos;
                _highlighter.gameObject.SetActive(true);
            }
            else
            {
                _highlighter.gameObject.SetActive(false);
            }
            Data.UpdateHighlighter(_highlighter, cell);
            
            if (Data.PlayType == CardPlayType.Anywhere)
                transform.position = eventData.position;
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            if (_highlighter != null)
            {
                Destroy(_highlighter.gameObject);
                _highlighter = null;
            }
            if (!_isBeingDragged)
                return;
            
            switch (Data.PlayType)
            {
                case CardPlayType.Cell:
                {
                    if (TargetingArrow.Instance.TryGetCell(out Vector2Int cell))
                        _playerHandManager.OnCardPlayed(this, cell);
                
                    TargetingArrow.Instance.Deactivate();
                    break;
                }
                case CardPlayType.Anywhere:
                    _playerHandManager.OnCardPlayed(this, Vector2Int.zero);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _isBeingDragged = false;
        }


        private void MoveTowardsHome()
        {
            transform.position = Vector3.Lerp(transform.position, _homePosition, Time.deltaTime * 10f);
        }
    }
}