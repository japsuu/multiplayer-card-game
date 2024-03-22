namespace Boards
{
    /// <summary>
    /// In-memory representation of a cell on the board.
    /// Contained in a <see cref="Board"/>.
    /// </summary>
    public class BoardCell
    {
        public readonly int X;
        public readonly int Y;
        
        /// <summary>
        /// The side of the grid the cell is on - player or enemy.
        /// </summary>
        public readonly CellSide Side;
        
        /// <summary>
        /// Whether the cell is occupied by a character or not.
        /// </summary>
        public bool IsOccupied;


        public BoardCell(int x, int y, CellSide side)
        {
            X = x;
            Y = y;
            Side = side;
        }
    }
}