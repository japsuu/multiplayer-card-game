using System;
using System.Collections;
using System.Collections.Generic;
using Cameras;
using Entities.Enemies;
using Entities.Players;
using UnityEngine;
using Utils.Singletons;

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
        /// <summary>
        /// Event that is invoked when the board is updated in any way.
        /// Used to for example invalidate cell highlighters. 
        /// </summary>
        public static event Action BoardUpdated;
        
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
        
        [Header("Enemies")]
        [SerializeField] private EnemyCharacter _enemyPrefab;
        [SerializeField] private Vector2Int _enemySpawnPosition;

        private BoardRenderer _boardRenderer;
        private Board _board;
        private CellHighlighter _hoveredCellHighlighter;


        public void Initialize()
        {
            _hoveredCellHighlighter = Instantiate(_hoveredCellHighlightPrefab, transform);
            _hoveredCellHighlighter.gameObject.SetActive(false);

            SetCameraOrigin();

            CreateLocalPlayer();

            CreateEnemies();
        }
        
        
        public bool TryGetCell(Vector2Int pos, out BoardCell cell) => TryGetCell(pos.x, pos.y, out cell);
        public bool TryGetCell(int x, int y, out BoardCell cell) => _board.TryGetCell(x, y, out cell);
        public bool IsCellEmpty(Vector2Int pos) => TryGetCell(pos, out BoardCell cell) && !cell.IsOccupied;


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


        public IEnumerable<BoardCell> GetEmptyCells(Vector2Int position, int range, CellSide side, bool includeOrigin = false)
        {
            if (range == 0)
                yield break;
            
            for (int y = position.y - range; y <= position.y + range; y++)
            for (int x = position.x - range; x <= position.x + range; x++)
            {
                Vector2Int cellPos = new(x, y);
                
                // Skip the origin
                if (!includeOrigin && cellPos == position)
                    continue;
                
                // Skip cells that are out of bounds
                if (!_board.TryGetCell(cellPos, out BoardCell cell))
                    continue;

                if (cell.IsOccupied || cell.Side != side)
                    continue;
                
                yield return cell;
            }
        }


        /// <summary>
        /// Traverses cells towards direction from the origin, stops at the last cell before out of bounds.
        /// </summary>
        /// <param name="position">The origin position to start traversing from. Not included in the traversal.</param>
        /// <param name="direction">The direction to traverse in.</param>
        /// <returns></returns>
        public IEnumerable<BoardCell> TraverseCells(Vector2Int position, GridDirection direction)
        {
            switch (direction)
            {
                case GridDirection.Up:
                {
                    int x = position.x;
                    for (int y = position.y + 1; y < _boardHeight; y++)
                    {
                        Vector2Int cellPos = new(x, y);
                
                        if (!_board.TryGetCell(cellPos, out BoardCell cell))
                            yield break;

                        yield return cell;
                    }
                    break;
                }
                case GridDirection.Down:
                {
                    int x = position.x;
                    for (int y = position.y - 1; y >= 0; y--)
                    {
                        Vector2Int cellPos = new(x, y);
                
                        if (!_board.TryGetCell(cellPos, out BoardCell cell))
                            yield break;

                        yield return cell;
                    }

                    break;
                }
                case GridDirection.Left:
                {
                    int y = position.y;
                    for (int x = position.x - 1; x >= 0; x--)
                    {
                        Vector2Int cellPos = new(x, y);
                
                        if (!_board.TryGetCell(cellPos, out BoardCell cell))
                            yield break;

                        yield return cell;
                    }
                    break;
                }
                case GridDirection.Right:
                {
                    int y = position.y;
                    for (int x = position.x + 1; x < _boardWidth; x++)
                    {
                        Vector2Int cellPos = new(x, y);
                
                        if (!_board.TryGetCell(cellPos, out BoardCell cell))
                            yield break;

                        yield return cell;
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
        
        
        public void HighlightCells(IEnumerable<Vector2Int> positions, bool isBlocked)
        {
            foreach (Vector2Int pos in positions)
            {
                HighlightCell(pos, isBlocked);
            }
        }
        
        
        public void HighlightCell(Vector2Int position, bool isBlocked)
        {
            _boardRenderer.HighlightCell(position, isBlocked);
        }


        public void StopHighlightCells()
        {
            _boardRenderer.StopHighlightCells();
        }
        
        
        public IEnumerator AddOccupant(Vector2Int pos, ICellOccupant occupant)
        {
            IEnumerator addOccupant = _board.AddOccupant(pos, occupant);
            return addOccupant;
        }


        public IEnumerator MoveOccupant(ICellOccupant occupant, Vector2Int to) => _board.MoveOccupant(occupant, to);
        public IEnumerator RemoveOccupant(ICellOccupant occupant) => _board.RemoveOccupant(occupant);


        private void Awake()
        {
            _boardRenderer = GetComponent<BoardRenderer>();
            
            int length = _enemySide == EnemyBoardSide.Top ? _boardHeight / 2 : _boardWidth / 2;
            _board = new Board(_boardWidth, _boardHeight, length, _enemySide);
            _board.BoardUpdated += OnBoardUpdated;
            
            _boardRenderer.InitializeGrid(_board);
        }


        private void OnBoardUpdated()
        {
            BoardUpdated?.Invoke();
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

            StartCoroutine(AddOccupant(_playerSpawnPosition, localPlayer));
        }


        private void CreateEnemies()
        {
            Vector3 worldPosition = _boardRenderer.CellToWorld(new Vector3Int(_enemySpawnPosition.x, _enemySpawnPosition.y, 0));
            EnemyCharacter enemy = Instantiate(_enemyPrefab, worldPosition, Quaternion.identity);
            
            StartCoroutine(AddOccupant(_enemySpawnPosition, enemy));
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
            DrawEnemySpawnGizmo();
        }


        private void DrawPlayerSpawnGizmo()
        {
            Vector3 spawnPos = new(_playerSpawnPosition.x + 0.5f, _playerSpawnPosition.y + 0.5f, 0);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPos, 0.5f);
        }


        private void DrawEnemySpawnGizmo()
        {
            Vector3 spawnPos = new(_enemySpawnPosition.x + 0.5f, _enemySpawnPosition.y + 0.5f, 0);
            Gizmos.color = Color.red;
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