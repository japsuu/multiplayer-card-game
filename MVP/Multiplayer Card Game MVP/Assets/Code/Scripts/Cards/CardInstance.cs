using System;
using System.Collections.Generic;
using Boards;
using Cards.Data;
using PhaseSystem;
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
        
        // [SerializeField]
        // private TMP_Text _manaCostText;
        
        [SerializeField]
        private TMP_Text _descriptionText;
        
        [SerializeField]
        private Image _artImage;

        [SerializeField]
        private Button _discardButton;
        
        private Vector3 _homePosition;
        private bool _isBeingDragged;
        private ICardInstanceReceiver _belowReceiver;
        private readonly List<RaycastResult> _raycastResults = new();
        
        [HideInInspector] public CellHighlightGroup CellHighlighter;
        
        public CardData Data { get; private set; }
        
        /// <summary>
        /// True if the card has been activated (placed to a card activation slot), false otherwise.
        /// Only active cards can be played.
        /// When an activated card is dragged, a targeting arrow will be shown instead of dragging the card.
        /// </summary>
        public bool HasBeenActivated { get; private set; }


        public void Initialize(CardData data)
        {
            Data = data;
            
            _nameText.text = data.CardName;
            // _manaCostText.text = data.ManaCost.ToString();
            _descriptionText.text = data.Description;
            _artImage.sprite = data.Sprite;
            
            _discardButton.onClick.AddListener(Discard);
            
            SetAllowDiscard(false);
        }
        
        
        public void Activate()
        {
            HasBeenActivated = true;
            Destroy(_discardButton.gameObject);     // Just in case
        }


        /// <summary>
        /// Called when the card is removed from a card activation slot.
        /// This will cause the card to be placed in the discard pile.
        /// </summary>
        public void DeactivateAndDiscard()
        {
            if (!HasBeenActivated)
                throw new Exception("Card is not activated.");
            
            HasBeenActivated = false;
            Discard();
        }
        
        
        public void SetAllowDiscard(bool allow)
        {
            if (HasBeenActivated)
                throw new Exception("Card is activated.");
            
            _discardButton.gameObject.SetActive(allow);
        }
        
        
        public void UpdateHomePosition(Vector3 homePosition)
        {
            if (HasBeenActivated)
                throw new Exception("Card is activated.");

            _homePosition = homePosition;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!GameLoopManager.AllowCardDragging)
                return;
            
            if (HasBeenActivated)
            {
                if (Data.CanBePlayed)
                {
                    // Create a highlighter group for the attack pattern of this card.
                    CellHighlightGroup highlighterGroup = BoardManager.Instance.CreateHighlightGroup();
                    highlighterGroup.SetPulseSettings(Boards.CellHighlighter.PulseSettings.Attack);
            
                    // Add highlighters for each cell in the attack pattern.
                    foreach (Vector2Int relativePosition in Data.CellPattern.GetCells(Vector2Int.zero))
                        highlighterGroup.AddRelativeHighlighter(relativePosition, Color.yellow);
            
                    // Assign the highlighter group to the dragged card.
                    CellHighlighter = highlighterGroup;
            
                    TargetingArrow.Instance.Activate(transform);
                }
                else
                {
                    Debug.LogWarning("TODO: Give user feedback that the card has no board effects.");
                    
                    // Cancel the dragging.
                    return;
                }
            }
            
            _isBeingDragged = true;
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (!_isBeingDragged)
                return;

            if (HasBeenActivated)
            {
                if (TargetingArrow.Instance.TryGetCell(out Vector2Int cell))
                {
                    BoardManager.Instance.TryGetCellToWorld(cell, out Vector3 pos);
                    CellHighlighter.transform.position = pos;
                    CellHighlighter.gameObject.SetActive(true);
                }
                else
                {
                    CellHighlighter.gameObject.SetActive(false);
                }
            }
            else
            {
                MoveTowardsCursor();
            }
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            DestroyHighlighter();
            
            if (!_isBeingDragged)
                return;

            if (HasBeenActivated)
            {
                if (Data.CanBePlayed)
                {
                    if (TargetingArrow.Instance.TryGetCell(out Vector2Int cell))
                        PlayerHandManager.Instance.PlayCard(this, cell);
            
                    TargetingArrow.Instance.Deactivate();
                }
                else
                    throw new Exception("Card has no board effects.");
            }
            else
            {
                if (_belowReceiver != null && _belowReceiver.CanReceiveCard(this))
                {
                    PlayerHandManager.Instance.RemoveCardFromHand(this);
                    _belowReceiver.ReceiveCard(this);
                }
            }
            
            StopDragging();
        }


        private void Discard()
        {
            PlayerHandManager.Instance.DiscardCard(this);
        }


        private void MoveTowardsCursor()
        {
            transform.position = (Vector2)Input.mousePosition;
        }


        private void StopDragging()
        {
            _belowReceiver?.OnHoverExit(this);
            _isBeingDragged = false;
        }


        private void Update()
        {
            if (_isBeingDragged)
            {
                RaycastForReceiver();

                _belowReceiver?.OnHover(this);

                CheckForDragCancellation();
                return;
            }
            
            if (HasBeenActivated)
                return;
            
            MoveTowardsHome();
        }


        private void RaycastForReceiver()
        {
            _raycastResults.Clear();
            
            EventSystem eventSystem = EventSystem.current;
            PointerEventData pointerEventData = new(eventSystem)
            {
                position = Input.mousePosition
            };

            eventSystem.RaycastAll(pointerEventData, _raycastResults);
            
            foreach (RaycastResult result in _raycastResults)
            {
                if (!result.gameObject.TryGetComponent(out ICardInstanceReceiver receiver))
                    continue;
                
                if (receiver == _belowReceiver)
                    return;

                _belowReceiver?.OnHoverExit(this);

                receiver.OnHoverEnter(this);
                _belowReceiver = receiver;
                    
                return;
            }

            if (_belowReceiver == null)
                return;
            
            _belowReceiver.OnHoverExit(this);
            _belowReceiver = null;
        }


        private void CheckForDragCancellation()
        {
            if (!Input.GetMouseButtonDown(1))
                return;
            
            if (Data.CanBePlayed)
                TargetingArrow.Instance.Deactivate();
            StopDragging();

            DestroyHighlighter();
        }


        private void DestroyHighlighter()
        {
            if (CellHighlighter == null)
                return;
            
            Destroy(CellHighlighter.gameObject);
            CellHighlighter = null;
        }


        private void MoveTowardsHome()
        {
            transform.position = Vector3.Lerp(transform.position, _homePosition, Time.deltaTime * 10f);
        }
    }
}