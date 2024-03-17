using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Grids
{
    /// <summary>
    /// Renders the gameplay grid by placing alternating colored cells in a chess-pattern on a tile-map.
    /// </summary>
    public class GridRenderer : MonoBehaviour
    {
        [Header("Tilemap")]
        [SerializeField]
        private Tilemap _renderTilemap;

        [SerializeField]
        private Tile _playerCellTile;

        [SerializeField]
        private Tile _enemyCellTile;

        [SerializeField]
        private Tile _baseCellTile;

        [Header("Cell highlighting")]
        [SerializeField]
        [Tooltip("Spawned over cells the player may move to.")]
        private CellHighlighter _availableCellHighlightPrefab;

        private GridData _gridData;
        private readonly List<CellHighlighter> _highlightedCells = new();


        public void InitializeGrid(GridData gridData)
        {
            _gridData = gridData;
            _gridData.OnGridUpdated += InitializeRenderGrid;
            InitializeRenderGrid();
        }


        private void InitializeRenderGrid()
        {
            for (int y = 0; y < _gridData.Height; y++)
            for (int x = 0; x < _gridData.Width; x++)
            {
                Tile tile = DetermineTileType(x, y);

                _renderTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }


        private Tile DetermineTileType(int x, int y)
        {
            Tile tile;
            if (x % 2 == 0)
                tile = y % 2 == 0 ? _baseCellTile : _playerCellTile;
            else
                tile = y % 2 == 0 ? _enemyCellTile : _baseCellTile;

            return tile;
        }


        public void StartHighlightCells(IEnumerable<Vector2Int> cells)
        {
            foreach (Vector2Int cell in cells)
            {
                CellHighlighter highlight = Instantiate(_availableCellHighlightPrefab, transform);
                highlight.transform.position = new Vector3(cell.x, cell.y, 0);
                _highlightedCells.Add(highlight);
            }
        }


        public void StopHighlightCells()
        {
            foreach (CellHighlighter highlight in _highlightedCells)
                Destroy(highlight.gameObject);
            _highlightedCells.Clear();
        }


        public Vector3Int WorldToCell(Vector3 mousePos)
        {
            return _renderTilemap.WorldToCell(mousePos);
        }


        public Vector3 CellToWorld(Vector3Int cellPos)
        {
            return _renderTilemap.GetCellCenterWorld(cellPos);
        }
    }
}