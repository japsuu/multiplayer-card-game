using System.Collections.Generic;
using Player;
using Singletons;
using UnityEngine;

namespace World.Grids
{
    /// <summary>
    /// Manages the grid the gameplay happens on.<br/>
    /// The grid is split into two parts: the player's side and the enemy's side.<br/>
    /// Players cannot move to the enemy's side and vice versa.<br/>
    /// </summary>
    [RequireComponent(typeof(GridRenderer))]
    public class GridManager : SingletonBehaviour<GridManager>
    {
        [Header("Grid")]
        
        [SerializeField]
        [Range(2, 12)]
        private int _gridWidth = 8;

        [SerializeField]
        [Range(2, 12)]
        private int _gridHeight = 4;

        [SerializeField]
        [Tooltip("Instantiated on game start. Not visible when the cursor is not hovering over the grid. When hovering the cursor over a grid cell, this prefab is snapped to the grid cell and set visible.")]
        private CellHighlighter _hoveredCellHighlightPrefab;

        [Header("Players")]
        
        [SerializeField]
        private PlayerCharacter _playerPrefab;
        
        [SerializeField]
        private Vector2Int _playerSpawnPosition;

        private GridRenderer _gridRenderer;
        private GridData _grid;
        private CellHighlighter _hoveredCellHighlighter;
        private Camera _camera;
        private readonly List<Vector2Int> _availableMovementCells = new();
        private readonly List<Vector2Int> _blockedMovementCells = new();


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

            RepositionCamera();

            CreateLocalPlayer();
        }


        private void RepositionCamera()
        {
            Vector3 gridCenter = new Vector3(_gridWidth / 2f, _gridHeight / 2f, -10f);
            _camera.transform.position = gridCenter;
        }


        private void CreateLocalPlayer()
        {
            Vector3 worldPosition = _gridRenderer.CellToWorld(new Vector3Int(_playerSpawnPosition.x, _playerSpawnPosition.y, 0));
            
            PlayerCharacter localPlayer = Instantiate(_playerPrefab, worldPosition, Quaternion.identity);
            PlayerCharacter.SetLocalPlayer(localPlayer);
            
            _grid.SetCellOccupation(_playerSpawnPosition.x, _playerSpawnPosition.y, true);
            localPlayer.SetGridPosition(_playerSpawnPosition, worldPosition);
        }


        private void Update()
        {
            UpdateHoveredCellHighlighter();
            CheckClickedCell();
        }


        private void CheckClickedCell()
        {
            if (PlayerCharacter.LocalPlayer == null)
                return;
            
            if (!Input.GetMouseButtonDown(0))
                return;
            
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            if (!TryGetWorldToCell(mousePos, out Vector2Int cellPos))
                return;
            
            HandleClickedCell(cellPos);
        }


        private void HandleClickedCell(Vector2Int clickedCellPos)
        {
            Vector2Int playerPos = PlayerCharacter.LocalPlayer.GridPosition;
            bool wasPlayerClicked = clickedCellPos == playerPos;
            bool hasHighlightedCells = _availableMovementCells.Count > 0 || _blockedMovementCells.Count > 0;
            
            // If the player was clicked and there are no highlighted cells, highlight the cells the player could move to.
            if (wasPlayerClicked && !hasHighlightedCells)
            {
                HighlightCellsAround(playerPos, PlayerCharacter.LocalPlayer.MovementRange);
                return;
            }
            
            // If a cell was clicked that can be moved to, move the player to that cell.
            if (_availableMovementCells.Contains(clickedCellPos))
            {
                PlayerCharacter.LocalPlayer.SetGridPosition(clickedCellPos, _gridRenderer.CellToWorld(new Vector3Int(clickedCellPos.x, clickedCellPos.y, 0)));
                _grid.SetCellOccupation(playerPos.x, playerPos.y, false);
                _grid.SetCellOccupation(clickedCellPos.x, clickedCellPos.y, true);
                
                StopHighlightCells();
                return;
            }
            
            // If any other cell was clicked, stop highlighting cells.
            StopHighlightCells();
        }


        private bool TryGetWorldToCell(Vector3 worldPos, out Vector2Int cellPos)
        {
            Vector3Int gridCellPos = _gridRenderer.WorldToCell(worldPos);
            if (gridCellPos.x < 0 || gridCellPos.x >= _grid.Width || gridCellPos.y < 0 || gridCellPos.y >= _grid.Height)
            {
                cellPos = new Vector2Int(-1, -1);
                return false;
            }

            cellPos = new Vector2Int(gridCellPos.x, gridCellPos.y);
            return true;
        }


        private void HighlightCellsAround(Vector2Int position, int range)
        {
            _availableMovementCells.Clear();
            _blockedMovementCells.Clear();
            for (int y = position.y - range; y <= position.y + range; y++)
            for (int x = position.x - range; x <= position.x + range; x++)
            {
                Vector2Int cellPos = new(x, y);
                
                // Skip cells that are out of bounds.
                if (x < 0 || x >= _grid.Width || y < 0 || y >= _grid.Height)
                    continue;
                
                // Skip the cell the player is on.
                if (cellPos == position)
                    continue;

                if (_grid.IsCellOccupied(x, y) || _grid.GetCellSide(x, y) != CellSide.Player)
                {
                    _gridRenderer.HighlightCell(cellPos, true);
                    _blockedMovementCells.Add(cellPos);
                }
                else
                {
                    _gridRenderer.HighlightCell(cellPos, false);
                    _availableMovementCells.Add(cellPos);
                }
            }
        }


        private void StopHighlightCells()
        {
            _gridRenderer.StopHighlightCells();
            _availableMovementCells.Clear();
            _blockedMovementCells.Clear();
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
            DrawPlayerSpawnGizmo();
        }


        private void DrawPlayerSpawnGizmo()
        {
            Vector3 spawnPos = new(_playerSpawnPosition.x + 0.5f, _playerSpawnPosition.y + 0.5f, 0);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPos, 0.5f);
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
            // ReSharper disable once PossibleLossOfFraction
            Vector3 divStart = origin + new Vector3(_gridWidth / 2, -1, 0f);
            Vector3 divEnd = divStart + Vector3.up * (_gridHeight + 2);
            Gizmos.DrawLine(divStart, divEnd);
        }
    }
}