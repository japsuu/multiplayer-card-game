using System.Collections;
using System.Collections.Generic;
using Boards;
using Cameras;
using UnityEngine;

namespace Entities.Players
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 5)]
        private int _movementRange = 2;
        
        /*[SerializeField]
        [Range(1, 5)]
        private int _movementActions = 1;*/
        
        private bool _allowMovement = true;
        private int _movementRangeModifier;
        
        private readonly List<Vector2Int> _availableMovementCells = new();
        
        public bool HasPlayerMoved { get; private set; }
        
        
        /// <summary>
        /// Allows player movement, and highlights the cells the player can move to.
        /// </summary>
        public void EnableMovement()
        {
            HasPlayerMoved = false;
            _allowMovement = true;
            StartHighlightCells(PlayerCharacter.LocalPlayer.BoardPosition);
        }
        
        
        /// <summary>
        /// Disables player movement, and stops highlighting the cells the player can move to.
        /// </summary>
        public void DisableMovement()
        {
            HasPlayerMoved = false;
            _allowMovement = false;
            StopHighlightCells();
        }
        
        
        public void ChangeMovementRangeModifier(int modifier)
        {
            _movementRangeModifier += modifier;
            print($"Player movement range modifier changed to: {_movementRangeModifier}");
        }

        
        private void Update()
        {
            if (!_allowMovement)
                return;
            
            CheckClickedCell();
        }


        private void CheckClickedCell()
        {
            if (PlayerCharacter.LocalPlayer == null)
                return;
            
            if (!Input.GetMouseButtonDown(0))
                return;
            
            Vector3 mousePos = CameraController.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
            if (!BoardManager.Instance.TryGetWorldToCell(mousePos, out Vector2Int cellPos))
                return;
            
            HandleClickedCell(cellPos);
        }


        private void HandleClickedCell(Vector2Int clickedCellPos)
        {
            bool hasHighlightedCells = _availableMovementCells.Count > 0;
            
            if (!hasHighlightedCells)
                return;
            
            if (!_availableMovementCells.Contains(clickedCellPos))
                return;
            
            // If a cell was clicked that can be moved to, move the player to that cell.
            MovePlayer(clickedCellPos);
                
            // Stop highlighting the cells.
            StopHighlightCells();
        }
        
        
        private int GetMovementRange()
        {
            return _movementRange + _movementRangeModifier;
        }


        private void MovePlayer(Vector2Int cellPos)
        {
            StartCoroutine(MoveCoroutine(cellPos));
        }


        private IEnumerator MoveCoroutine(Vector2Int cellPos)
        {
            yield return BoardManager.Instance.MoveOccupant(PlayerCharacter.LocalPlayer, cellPos);
            HasPlayerMoved = true;
            /*if (PlayerMovementsThisTurn < _movementActions)
                StartHighlightCells(cellPos);*/
        }


        private void StartHighlightCells(Vector2Int playerPos)
        {
            StopHighlightCells();

            foreach (BoardCell cell in BoardManager.Instance.GetEmptyCells(playerPos, GetMovementRange(), CellSide.Player))
            {
                _availableMovementCells.Add(cell.Position);
                BoardManager.Instance.HighlightCell(cell.Position, false);
            }
        }


        private void StopHighlightCells()
        {
            BoardManager.Instance.StopHighlightCells();
            _availableMovementCells.Clear();
        }
    }
}