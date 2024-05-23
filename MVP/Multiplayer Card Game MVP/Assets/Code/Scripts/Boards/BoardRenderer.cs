using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Boards
{
    /// <summary>
    /// Renders the gameplay board by placing alternating colored cells in a chess-pattern on a tile-map.
    /// </summary>
    public class BoardRenderer : MonoBehaviour
    {
        [Header("Tilemap")]
        [SerializeField]
        private Tilemap _renderTilemap;

        [SerializeField]
        private Tile _playerCellTile1;

        [SerializeField]
        private Tile _playerCellTile2;

        [SerializeField]
        private Tile _enemyCellTile1;

        [SerializeField]
        private Tile _enemyCellTile2;

        [Header("Cell highlighting")]
        
        [SerializeField]
        [Tooltip("Spawned over cells the player may move to.")]
        private CellHighlighter _emptyCellHighlightPrefab;

        [SerializeField]
        [Tooltip("Spawned over cells the player may NOT move to.")]
        private CellHighlighter _blockedCellHighlightPrefab;

        [SerializeField]
        private CellHighlightGroup _highlightGroupPrefab;

        private Board _board;
        private readonly List<CellHighlighter> _highlightedCells = new();


        public void InitializeGrid(Board board)
        {
            _board = board;
            _board.BoardSizeChanged += InitializeRenderBoard;
            InitializeRenderBoard();
        }
        
        
        /// <summary>
        /// Highlights the defined cell on the board.
        /// </summary>
        /// <param name="cell">The cell to highlight.</param>
        /// <param name="isBlocked">Whether the cell is blocked (contains something) or not.</param>
        public void HighlightCell(Vector2Int cell, bool isBlocked)
        {
            CellHighlighter highlight = isBlocked ? _blockedCellHighlightPrefab : _emptyCellHighlightPrefab;
            CellHighlighter newHighlight = Instantiate(highlight, transform);
            Vector3 cellWorldPos = CellToWorld((Vector3Int)cell);
            newHighlight.transform.position = cellWorldPos;
            _highlightedCells.Add(newHighlight);
        }


        /// <summary>
        /// Stops highlighting all cells on the board.
        /// </summary>
        public void StopHighlightCells()
        {
            foreach (CellHighlighter highlight in _highlightedCells)
                Destroy(highlight.gameObject);
            _highlightedCells.Clear();
        }
        
        
        /// <summary>
        /// Creates a new highlight group.
        /// </summary>
        /// <returns></returns>
        public CellHighlightGroup CreateHighlightGroup()
        {
            return Instantiate(_highlightGroupPrefab, transform);
        }


        /// <summary>
        /// Gets the cell position in the world space.
        /// </summary>
        /// <param name="mousePos">The world (usually mouse) position.</param>
        /// <returns>The cell position in the world space.</returns>
        public Vector3Int WorldToCell(Vector3 mousePos)
        {
            return _renderTilemap.WorldToCell(mousePos);
        }


        /// <summary>
        /// Inverse of <see cref="WorldToCell"/>.
        /// </summary>
        public Vector3 CellToWorld(Vector3Int cellPos)
        {
            return _renderTilemap.GetCellCenterWorld(cellPos);
        }


        private void InitializeRenderBoard()
        {
            for (int y = 0; y < _board.Height; y++)
            for (int x = 0; x < _board.Width; x++)
            {
                Tile tile = DetermineTileType(x, y);

                _renderTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }


        private Tile DetermineTileType(int x, int y)
        {
            bool isPlayer = _board.GetCellSide(x, y) == CellSide.Player;
            
            bool alternator = (x + y) % 2 == 0;
            if (isPlayer)
                return alternator ? _playerCellTile1 : _playerCellTile2;
            return alternator ? _enemyCellTile1 : _enemyCellTile2;
        }
    }
}