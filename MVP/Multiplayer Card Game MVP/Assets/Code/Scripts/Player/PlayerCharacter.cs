using System;
using Boards;
using DamageSystem;
using PhaseSystem;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Represents a player character on the board.
    /// </summary>
    [RequireComponent(typeof(EntityHealth))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerCharacter : MonoBehaviour, ICellOccupant
    {
        public static PlayerCharacter LocalPlayer { get; private set; }
        
        public static event Action<PlayerCharacter> LocalPlayerCreated;
        public static event Action LocalPlayerDestroyed;
        
        public PlayerMovement Movement { get; private set; }
        public EntityHealth Health { get; private set; }
        public Vector2Int BoardPosition { get; private set; }
        
        public bool IsLocalPlayer => this == LocalPlayer;
        public IDamageable Damageable => Health;


        public static void SetLocalPlayer(PlayerCharacter player)
        {
            LocalPlayer = player;
            LocalPlayerCreated?.Invoke(player);
        }


        public void OnAddedToBoard(Vector2Int boardPos)
        {
            MoveTo(boardPos);
        }


        public void OnMovedOnBoard(Vector2Int newBoardPos)
        {
            MoveTo(newBoardPos);
        }


        public void OnRemovedFromBoard()
        {
            throw new NotImplementedException();
        }


        private void Awake()
        {
            Health = GetComponent<EntityHealth>();
            Movement = GetComponent<PlayerMovement>();
            Health.Died += OnDied;
        }


        private void MoveTo(Vector2Int boardPos)
        {
            BoardPosition = boardPos;
            
            if (!BoardManager.Instance.TryGetCellToWorld(boardPos, out Vector3 worldPos))
                throw new Exception("Could not get world position of cell.");
            
            transform.position = worldPos;
        }


        private void OnDied()
        {
            BoardManager.Instance.RemoveOccupant(this);
            GameLoopManager.Instance.StopGameLoop();
            
            Destroy(gameObject);
        }


        private void OnDestroy()
        {
            if (!IsLocalPlayer)
                return;
            
            LocalPlayerDestroyed?.Invoke();
            LocalPlayer = null;
        }
    }
}