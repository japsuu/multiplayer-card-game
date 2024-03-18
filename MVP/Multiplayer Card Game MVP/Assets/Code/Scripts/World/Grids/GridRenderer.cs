using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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

        [FormerlySerializedAs("_availableCellHighlightPrefab")]
        [Header("Cell highlighting")]
        [SerializeField]
        [Tooltip("Spawned over cells the player may move to.")]
        private CellHighlighter _emptyCellHighlightPrefab;

        [SerializeField]
        [Tooltip("Spawned over cells the player may NOT move to.")]
        private CellHighlighter _blockedCellHighlightPrefab;

        private GridData _gridData;
        private readonly List<CellHighlighter> _highlightedCells = new();


        public void InitializeGrid(GridData gridData)
        {
            _gridData = gridData;
            _gridData.OnGridUpdated += InitializeRenderGrid;
            InitializeRenderGrid();
        }
        
        
        public void HighlightCell(Vector2Int cell, bool isBlocked)
        {
            CellHighlighter highlight = isBlocked ? _blockedCellHighlightPrefab : _emptyCellHighlightPrefab;
            CellHighlighter newHighlight = Instantiate(highlight, transform);
            Vector3 cellWorldPos = CellToWorld((Vector3Int)cell);
            newHighlight.transform.position = cellWorldPos;
            _highlightedCells.Add(newHighlight);
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
            bool isBaseCell = (x + y) % 2 == 0;
            if (isBaseCell)
                return _baseCellTile;

            bool isPlayer = _gridData.GetCellSide(x, y) == CellSide.Player;
            
            return isPlayer ? _playerCellTile : _enemyCellTile;
        }
    }
}