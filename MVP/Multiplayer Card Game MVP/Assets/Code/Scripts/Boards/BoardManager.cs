using System.Collections.Generic;
using Cameras;
using PhaseSystem;
using Player;
using Singletons;
using UnityEngine;

namespace Boards
{
    /// <summary>
    /// Manages the board the gameplay happens on.<br/>
    /// The board is split into two parts: the player's side and the enemy's side.<br/>
    /// Players cannot move to the enemy's side and vice versa.<br/>
    /// </summary>
    [RequireComponent(typeof(BoardRenderer))]
    public class BoardManager : SingletonBehaviour<BoardManager>
    {
        [Header("Board")]
        
        [SerializeField]
        [Range(2, 12)]
        private int _boardWidth = 5;

        [SerializeField]
        [Range(2, 12)]
        private int _boardHeight = 8;

        [SerializeField]
        private EnemyBoardSide _enemySide = EnemyBoardSide.Top;

        [SerializeField]
        [Tooltip("Instantiated on game start. Not visible when the cursor is not hovering over the board. When hovering the cursor over a board cell, this prefab is snapped to the board cell and set visible.")]
        private CellHighlighter _hoveredCellHighlightPrefab;

        [Header("Players")]
        [SerializeField] private PlayerCharacter _playerPrefab;
        [SerializeField] private Vector2Int _playerSpawnPosition;

        private BoardRenderer _boardRenderer;
        private Board _board;
        private CellHighlighter _hoveredCellHighlighter;
        
        
        public bool TryGetCell(Vector2Int pos, out BoardCell cell) => TryGetCell(pos.x, pos.y, out cell);
        public bool TryGetCell(int x, int y, out BoardCell cell) => _board.TryGetCell(x, y, out cell);


        /// <summary>
        /// Tries to snap the given world position to the nearest board cell position.
        /// </summary>
        /// <param name="worldPos">The world position to snap to a board cell.</param>
        /// <param name="cellPos">The snapped board cell position.</param>
        /// <returns>If the world position was snapped to a board cell (if the position was within the board).</returns>
        public bool TrySnapToCell(Vector3 worldPos, out Vector3 cellPos)
        {
            if (!TryGetWorldToCell(worldPos, out Vector2Int boardCellPos))
            {
                cellPos = Vector3.zero;
                return false;
            }

            cellPos = _boardRenderer.CellToWorld(new Vector3Int(boardCellPos.x, boardCellPos.y, 0));
            return true;
        }


        /// <summary>
        /// Tries to get the board cell position from the given world position.
        /// </summary>
        /// <param name="worldPos">The world position to get the board cell position from.</param>
        /// <param name="cellPos">The board cell position.</param>
        /// <returns>If the world position was within the board.</returns>
        public bool TryGetWorldToCell(Vector3 worldPos, out Vector2Int cellPos)
        {
            Vector3Int boardCellPos = _boardRenderer.WorldToCell(worldPos);
            if (boardCellPos.x < 0 || boardCellPos.x >= _board.Width || boardCellPos.y < 0 || boardCellPos.y >= _board.Height)
            {
                cellPos = new Vector2Int(-1, -1);
                return false;
            }
            
            cellPos = new Vector2Int(boardCellPos.x, boardCellPos.y);
            return true;
        }
        
        
        /// <summary>
        /// Inverse of <see cref="TryGetWorldToCell"/>.
        /// </summary>
        public bool TryGetCellToWorld(Vector2Int cellPos, out Vector3 worldPos)
        {
            if (cellPos.x < 0 || cellPos.x >= _board.Width || cellPos.y < 0 || cellPos.y >= _board.Height)
            {
                worldPos = Vector3.zero;
                return false;
            }

            worldPos = _boardRenderer.CellToWorld(new Vector3Int(cellPos.x, cellPos.y, 0));
            return true;
        }
        
        
        /// <summary>
        /// Creates a new highlight group.
        /// </summary>
        public CellHighlightGroup CreateHighlightGroup()
        {
            return _boardRenderer.CreateHighlightGroup();
        }


        public IEnumerable<BoardCell> HighlightCellsForMovement(Vector2Int position, int range)
        {
            for (int y = position.y - range; y <= position.y + range; y++)
            for (int x = position.x - range; x <= position.x + range; x++)
            {
                Vector2Int cellPos = new(x, y);
                
                // Skip the cell the player is on.
                if (cellPos == position)
                    continue;
                
                // Skip cells that are out of bounds.
                if (!_board.TryGetCell(x, y, out BoardCell cell))
                    continue;

                if (cell.IsOccupied || cell.Side != CellSide.Player)
                {
                    _boardRenderer.HighlightCell(cellPos, true);
                }
                else
                {
                    _boardRenderer.HighlightCell(cellPos, false);
                }
                
                yield return cell;
            }
        }


        public void StopHighlightCells()
        {
            _boardRenderer.StopHighlightCells();
        }
        
        
        public void AddOccupant(Vector2Int pos, ICellOccupant occupant) => _board.AddOccupant(pos, occupant);
        public void MoveOccupant(ICellOccupant occupant, Vector2Int to) => _board.MoveOccupant(occupant, to);
        public void RemoveOccupant(ICellOccupant occupant) => _board.RemoveOccupant(occupant);


        private void Awake()
        {
            _boardRenderer = GetComponent<BoardRenderer>();
            
            int length = _enemySide == EnemyBoardSide.Top ? _boardHeight / 2 : _boardWidth / 2;
            _board = new Board(_boardWidth, _boardHeight, length, _enemySide);
            
            _boardRenderer.InitializeGrid(_board);
        }


        private void Start()
        {
            _hoveredCellHighlighter = Instantiate(_hoveredCellHighlightPrefab, transform);
            _hoveredCellHighlighter.gameObject.SetActive(false);

            SetCameraOrigin();

            CreateLocalPlayer();
            
            GameLoopManager.Instance.StartGameLoop();
        }


        private void SetCameraOrigin()
        {
            CameraController.Instance.SetOrigin(new Vector2(_boardWidth / 2f, _boardHeight / 2f));
        }


        private void CreateLocalPlayer()
        {
            Vector3 worldPosition = _boardRenderer.CellToWorld(new Vector3Int(_playerSpawnPosition.x, _playerSpawnPosition.y, 0));
            
            PlayerCharacter localPlayer = Instantiate(_playerPrefab, worldPosition, Quaternion.identity);
            PlayerCharacter.SetLocalPlayer(localPlayer);
            
            AddOccupant(_playerSpawnPosition, localPlayer);
        }


        private void Update()
        {
            UpdateHoveredCellHighlighter();
        }


        private void UpdateHoveredCellHighlighter()
        {
            Vector3 mousePos = CameraController.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = _boardRenderer.WorldToCell(mousePos);
            if (cellPos.x < 0 || cellPos.x >= _board.Width || cellPos.y < 0 || cellPos.y >= _board.Height)
            {
                _hoveredCellHighlighter.gameObject.SetActive(false);
                return;
            }

            Vector3 cellWorldPos = _boardRenderer.CellToWorld(cellPos);
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
            int sideLength = _enemySide == EnemyBoardSide.Top ? _boardHeight : _boardWidth;
            int crossLength = _enemySide == EnemyBoardSide.Top ? _boardWidth : _boardHeight;
            int playerSideLength = sideLength / 2;

            Gizmos.color = Color.white;
            for (float x = 0; x <= _boardWidth; x++)
            {
                Vector3 start = origin + transform.right * x;
                Gizmos.DrawLine(start, start + transform.up * _boardHeight);
            }

            for (float y = 0; y <= _boardHeight; y++)
            {
                Vector3 start = origin + transform.up * y;
                Gizmos.DrawLine(start, start + transform.right * _boardWidth);
            }
            
            // Draw a yellow dividing line between the player and enemy sides.
            Gizmos.color = Color.yellow;

            Vector3 boardSideDirection = _enemySide == EnemyBoardSide.Top ? transform.up : transform.right;
            Vector3 boardCrossDirection = _enemySide == EnemyBoardSide.Top ? transform.right : transform.up;

            Vector3 divStart = origin + boardSideDirection * playerSideLength - boardCrossDirection;
            Vector3 divEnd = divStart + boardCrossDirection * (crossLength + 2);
            Gizmos.DrawLine(divStart, divEnd);
        }
    }
}