using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Boards
{
    /// <summary>
    /// Contains the data representation of a game board.
    /// Managed by <see cref="BoardManager"/>.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Called when the board size has changed.
        /// </summary>
        public event Action BoardSizeChanged;
        
        /// <summary>
        /// Called when the contents of a cell have changed.
        /// Arguments: The coordinates of the changed cell, and the new occupant of the cell.
        /// </summary>
        public event Action<Vector2Int, ICellOccupant> OnCellOccupationChanged;
        
        /// <summary>
        /// Width of the board in cells.
        /// </summary>
        public int Width { get; private set; }
        
        /// <summary>
        /// Height of the board in cells.
        /// </summary>
        public int Height { get; private set; }

        private BoardCell[,] _boardCells;


        /// <summary>
        /// Creates a new board.
        /// </summary>
        /// <param name="width">The width of the board in cells.</param>
        /// <param name="height">The height of the board in cells.</param>
        /// <param name="playerCellsLength">How many cells from the bottom-left of the board are the player's cells.</param>
        /// <param name="enemyBoardSide">Where on the board the enemy's cells are.</param>
        public Board(int width, int height, int playerCellsLength, EnemyBoardSide enemyBoardSide)
        {
            Resize(width, height, playerCellsLength, enemyBoardSide);
        }
        
        
        public bool TryGetCell(Vector2Int pos, out BoardCell cell) => TryGetCell(pos.x, pos.y, out cell);
        public bool TryGetCell(int x, int y, out BoardCell cell)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                cell = null;
                return false;
            }
            
            cell = _boardCells[x, y];
            return true;
        }
        
        
        /// <returns>If a cell is occupied or not.</returns>
        public bool IsCellOccupied(int x, int y) => _boardCells[x, y].Occupant != null;


        /// <returns>The side of the grid the cell is on - player or enemy.</returns>
        public CellSide GetCellSide(int x, int y) => _boardCells[x, y].Side;


        /// <summary>
        /// Resizes the board.
        /// </summary>
        /// <param name="width">The width of the board in cells.</param>
        /// <param name="height">The height of the board in cells.</param>
        /// <param name="playerCellsLength">How many cells from the bottom-left of the board are the player's cells.</param>
        /// <param name="enemyBoardSide">Where on the board the enemy's cells are.</param>
        public void Resize(int width, int height, int playerCellsLength, EnemyBoardSide enemyBoardSide)
        {
            Width = width;
            Height = height;
            InitializeGrid(width, height, playerCellsLength, enemyBoardSide);
        }


        public void AddOccupant(Vector2Int pos, [CanBeNull] ICellOccupant occupant)
        {
            if (!TryGetCell(pos, out BoardCell cell))
                Debug.LogError($"Could not get cell at {pos}.");
            
            if (cell.Occupant == occupant)
                return;
            
            if (cell.Occupant != null)
                Debug.LogWarning($"Cell at {pos} is already occupied, replacing...");
            
            cell.Occupant = occupant;
            
            occupant?.OnAddedToBoard(pos);
            
            OnCellOccupationChanged?.Invoke(pos, occupant);
        }


        public void MoveOccupant(ICellOccupant occupant, Vector2Int toPos)
        {
            Vector2Int fromPos = occupant.BoardPosition;
            
            if (!TryGetCell(fromPos, out BoardCell fromCell))
                Debug.LogError($"Could not get cell at {fromPos}.");
            
            if (!TryGetCell(toPos, out BoardCell toCell))
                Debug.LogError($"Could not get cell at {toPos}.");

            if (fromCell.Occupant != occupant)
            {
                Debug.LogError($"Cell at {fromPos} does not contain the expected occupant.");
                return;
            }
            
            if (toCell.Occupant != null)
                Debug.LogWarning($"Cell at {toPos} is already occupied, replacing...");
            
            toCell.Occupant = occupant;
            fromCell.Occupant = null;
            
            occupant.OnMovedOnBoard(toPos);
            
            OnCellOccupationChanged?.Invoke(fromPos, null);
            OnCellOccupationChanged?.Invoke(toPos, occupant);
        }
        
        
        public void RemoveOccupant(ICellOccupant occupant)
        {
            Vector2Int position = occupant.BoardPosition;
            
            if (!TryGetCell(position, out BoardCell cell))
                Debug.LogError($"Could not get cell at {position}.");

            if (cell.Occupant != occupant)
            {
                Debug.LogError($"Cell at {position} does not contain the expected occupant.");
                return;
            }
            
            cell.Occupant = null;
            occupant.OnRemovedFromBoard();
            
            OnCellOccupationChanged?.Invoke(position, null);
        }


        private void InitializeGrid(int width, int height, int playerCellsLength, EnemyBoardSide enemyBoardSide)
        {
            _boardCells = new BoardCell[width, height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int cellLength = enemyBoardSide == EnemyBoardSide.Top ? y : x;
                    CellSide side = cellLength < playerCellsLength ? CellSide.Player : CellSide.Enemy;
                    _boardCells[x, y] = new BoardCell(x, y, side);
                }
            }
            BoardSizeChanged?.Invoke();
        }


        public Vector2Int GetRandomEmptyCell(Vector2Int origin, int range, bool includeOrigin)
        {
#warning Hacky solution, replace with a better one.
            for (int i = 0; i < 100; i++)
            {
                int x = UnityEngine.Random.Range(origin.x - range, origin.x + range + 1);
                int y = UnityEngine.Random.Range(origin.y - range, origin.y + range + 1);
                
                if (!includeOrigin && x == origin.x && y == origin.y)
                    continue;
                
                if (TryGetCell(x, y, out BoardCell cell) && cell.Occupant == null)
                    return new Vector2Int(x, y);
            }
            
            throw new Exception("Could not find an empty cell.");
        }
    }
}