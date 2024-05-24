using System;
using System.Collections.Generic;
using Boards;
using Cameras;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 5)]
        private int _movementRange = 2;
        
        private bool _allowMovement = true;
        
        private readonly List<Vector2Int> _availableMovementCells = new();
        private readonly List<Vector2Int> _blockedMovementCells = new();
        
        public int PlayerMovementsThisTurn { get; private set; }
        
        
        /// <summary>
        /// Allows player movement, and highlights the cells the player can move to.
        /// </summary>
        public void EnableMovement()
        {
            PlayerMovementsThisTurn = 0;
            _allowMovement = true;
            StartHighlightCells(PlayerCharacter.LocalPlayer.BoardPosition);
        }
        
        
        /// <summary>
        /// Disables player movement, and stops highlighting the cells the player can move to.
        /// </summary>
        public void DisableMovement()
        {
            PlayerMovementsThisTurn = 0;
            _allowMovement = false;
            StopHighlightCells();
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
            /*Vector2Int playerPos = PlayerCharacter.LocalPlayer.BoardPosition;
            bool wasPlayerClicked = clickedCellPos == playerPos;
            // If the player was clicked and there are no highlighted cells, highlight the cells the player could move to.
            if (wasPlayerClicked && !hasHighlightedCells)
            {
                StartHighlightCells(playerPos);
                return;
            }*/
            
            bool hasHighlightedCells = _availableMovementCells.Count > 0 || _blockedMovementCells.Count > 0;
            
            if (!hasHighlightedCells)
                return;
            
            if (!_availableMovementCells.Contains(clickedCellPos))
                return;
            
            // If a cell was clicked that can be moved to, move the player to that cell.
            MovePlayer(clickedCellPos);
                
            // Stop highlighting the cells.
            StopHighlightCells();
        }


        private void MovePlayer(Vector2Int cellPos)
        {
            PlayerMovementsThisTurn++;
            BoardManager.Instance.MoveOccupant(PlayerCharacter.LocalPlayer, cellPos);
        }


        private void StartHighlightCells(Vector2Int playerPos)
        {
            _availableMovementCells.Clear();
            _blockedMovementCells.Clear();

            foreach (BoardCell cell in BoardManager.Instance.HighlightCellsForMovement(playerPos, _movementRange))
            {
                if (cell.IsOccupied || cell.Side != CellSide.Player)
                {
                    _blockedMovementCells.Add(new Vector2Int(cell.X, cell.Y));
                }
                else
                {
                    _availableMovementCells.Add(new Vector2Int(cell.X, cell.Y));
                }
            }
        }


        private void StopHighlightCells()
        {
            BoardManager.Instance.StopHighlightCells();
            _availableMovementCells.Clear();
            _blockedMovementCells.Clear();
        }
    }
}