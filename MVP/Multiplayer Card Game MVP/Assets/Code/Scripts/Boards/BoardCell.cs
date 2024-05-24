using UnityEngine;

namespace Boards
{
    /// <summary>
    /// In-memory representation of a cell on the board.
    /// Contained in a <see cref="Board"/>.
    /// </summary>
    public class BoardCell
    {
        public readonly Vector2Int Position;
        public readonly int X;
        public readonly int Y;
        
        /// <summary>
        /// The side of the grid the cell is on - player or enemy.
        /// </summary>
        public readonly CellSide Side;
        
        /// <summary>
        /// The occupant of the cell.
        /// </summary>
        public ICellOccupant Occupant;
        
        public bool IsOccupied => Occupant != null;


        public BoardCell(int x, int y, CellSide side)
        {
            X = x;
            Y = y;
            Position = new Vector2Int(x, y);
            Side = side;
        }
    }
}