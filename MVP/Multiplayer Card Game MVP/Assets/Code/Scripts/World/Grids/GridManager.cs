using System;
using UnityEngine;

namespace World.Grids
{
    /// <summary>
    /// Manages the grid the gameplay happens on.<br/>
    /// The grid is split into two parts: the player's side and the enemy's side.<br/>
    /// Players cannot move to the enemy's side and vice versa.<br/>
    /// </summary>
    [RequireComponent(typeof(GridRenderer))]
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        [Range(2, 12)]
        private int _gridWidth = 8;

        [SerializeField]
        [Range(2, 12)]
        private int _gridHeight = 4;

        [SerializeField]
        [Tooltip("Instantiated on game start. Not visible when the cursor is not hovering over the grid. When hovering the cursor over a grid cell, this prefab is snapped to the grid cell and set visible.")]
        private CellHighlighter _hoveredCellHighlightPrefab;

        private GridRenderer _gridRenderer;
        private GridData _grid;
        private CellHighlighter _hoveredCellHighlighter;
        private Camera _camera;


        private void Awake()
        {
            _camera = Camera.main;
            _gridRenderer = GetComponent<GridRenderer>();
            _grid = new GridData(_gridWidth, _gridHeight, _gridWidth / 2);
            _gridRenderer.InitializeGrid(_grid);
        }


        private void Start()
        {
            _hoveredCellHighlighter = Instantiate(_hoveredCellHighlightPrefab, transform);
            _hoveredCellHighlighter.gameObject.SetActive(false);
        }


        private void Update()
        {
            UpdateHoveredCellHighlighter();
        }


        private void UpdateHoveredCellHighlighter()
        {
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = _gridRenderer.WorldToCell(mousePos);
            if (cellPos.x < 0 || cellPos.x >= _grid.Width || cellPos.y < 0 || cellPos.y >= _grid.Height)
            {
                _hoveredCellHighlighter.gameObject.SetActive(false);
                return;
            }

            Vector3 cellWorldPos = _gridRenderer.CellToWorld(cellPos);
            _hoveredCellHighlighter.transform.position = cellWorldPos;
            _hoveredCellHighlighter.gameObject.SetActive(true);
        }


        private void OnDrawGizmos()
        {
            DrawGridGizmos();
        }


        private void DrawGridGizmos()
        {
            Vector3 origin = transform.position;

            Gizmos.color = Color.white;
            for (float x = 0; x <= _gridWidth; x++)
            {
                Vector3 start = origin + new Vector3(x, 0f, 0f);
                Gizmos.DrawLine(start, start + Vector3.up * _gridHeight);
            }

            for (float y = 0; y <= _gridHeight; y++)
            {
                Vector3 start = origin + new Vector3(0f, y, 0f);
                Gizmos.DrawLine(start, start + Vector3.right * _gridWidth);
            }
            
            // Draw a yellow dividing line between the player and enemy sides.
            Gizmos.color = Color.yellow;
            Vector3 divStart = origin + new Vector3(_gridWidth / 2, -1, 0f);
            Vector3 divEnd = divStart + Vector3.up * (_gridHeight + 2);
            Gizmos.DrawLine(divStart, divEnd);
        }
    }
}