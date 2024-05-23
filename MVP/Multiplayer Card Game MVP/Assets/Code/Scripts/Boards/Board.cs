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
        public bool IsCellOccupied(int x, int y)
        {
            return _boardCells[x, y].Occupant != null;
        }
        
        
        /// <returns>The side of the grid the cell is on - player or enemy.</returns>
        public CellSide GetCellSide(int x, int y)
        {
            return _boardCells[x, y].Side;
        }
        
        
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


        /// <summary>
        /// Sets the occupation of a cell (if it contains something or not).
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        /// <param name="occupant">The new occupant of the cell.</param>
        public void SetCellOccupation(int x, int y, [CanBeNull] ICellOccupant occupant)
        {
            if (_boardCells[x, y].Occupant == occupant)
                return;
            
            _boardCells[x, y].Occupant = occupant;
            
            OnCellOccupationChanged?.Invoke(new Vector2Int(x, y), occupant);
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
    }
}