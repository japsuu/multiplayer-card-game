using System;

namespace World.Grids
{
    /// <summary>
    /// Contains the data representation of the grid.
    /// </summary>
    public class GridData
    {
        public event Action OnGridUpdated;
        public event Action<int, int> OnCellOccupationChanged;
        
        public int Width { get; private set; }
        public int Height { get; private set; }

        private GridCell[,] _grid;


        public GridData(int width, int height, int playerCellsWidth)
        {
            Width = width;
            Height = height;

            InitializeGrid(width, height, playerCellsWidth);
        }
        
        
        public void UpdateGrid(int width, int height, int playerCellsWidth)
        {
            Width = width;
            Height = height;
            InitializeGrid(width, height, playerCellsWidth);
        }
        
        
        public void SetCellOccupation(int x, int y, bool isOccupied)
        {
            if (_grid[x, y].IsOccupied == isOccupied)
                return;
            
            _grid[x, y].IsOccupied = isOccupied;
            
            OnCellOccupationChanged?.Invoke(x, y);
        }
        
        
        public bool IsCellOccupied(int x, int y)
        {
            return _grid[x, y].IsOccupied;
        }
        
        
        public CellSide GetCellSide(int x, int y)
        {
            return _grid[x, y].Side;
        }


        private void InitializeGrid(int width, int height, int playerCellsWidth)
        {
            _grid = new GridCell[width, height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    CellSide side = x < playerCellsWidth ? CellSide.Player : CellSide.Enemy;
                    _grid[x, y] = new GridCell(x, y, side);
                }
            }
            OnGridUpdated?.Invoke();
        }
    }
}