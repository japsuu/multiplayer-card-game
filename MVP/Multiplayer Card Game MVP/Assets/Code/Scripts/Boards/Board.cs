using System;
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
        /// Arguments: The coordinates of the changed cell, and whether the cell is now occupied or not.
        /// </summary>
        public event Action<Vector2Int, bool> OnCellOccupationChanged;
        
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
        /// <param name="playerCellsWidth">How many cells from the left side of the board are the player's cells.</param>
        public Board(int width, int height, int playerCellsWidth)
        {
            Resize(width, height, playerCellsWidth);
        }
        
        
        /// <summary>
        /// Resizes the board.
        /// </summary>
        /// <param name="width">The width of the board in cells.</param>
        /// <param name="height">The height of the board in cells.</param>
        /// <param name="playerCellsWidth">How many cells from the left side of the board are the player's cells.</param>
        public void Resize(int width, int height, int playerCellsWidth)
        {
            Width = width;
            Height = height;
            InitializeGrid(width, height, playerCellsWidth);
        }
        
        
        /// <summary>
        /// Sets the occupation of a cell (if it contains something or not).
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        /// <param name="isOccupied">If the cell is occupied or not.</param>
        public void SetCellOccupation(int x, int y, bool isOccupied)
        {
            if (_boardCells[x, y].IsOccupied == isOccupied)
                return;
            
            _boardCells[x, y].IsOccupied = isOccupied;
            
            OnCellOccupationChanged?.Invoke(new Vector2Int(x, y), isOccupied);
        }
        
        
        /// <returns>If a cell is occupied or not.</returns>
        public bool IsCellOccupied(int x, int y)
        {
            return _boardCells[x, y].IsOccupied;
        }
        
        
        /// <returns>The side of the grid the cell is on - player or enemy.</returns>
        public CellSide GetCellSide(int x, int y)
        {
            return _boardCells[x, y].Side;
        }


        private void InitializeGrid(int width, int height, int playerCellsWidth)
        {
            _boardCells = new BoardCell[width, height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    CellSide side = x < playerCellsWidth ? CellSide.Player : CellSide.Enemy;
                    _boardCells[x, y] = new BoardCell(x, y, side);
                }
            }
            BoardSizeChanged?.Invoke();
        }
    }
}