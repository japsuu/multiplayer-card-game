using System;
using Boards;
using DamageSystem;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Represents a player character on the board.
    /// </summary>
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerCharacter : MonoBehaviour, ICellOccupant
    {
        public static PlayerCharacter LocalPlayer { get; private set; }
        
        public static event Action<PlayerCharacter> LocalPlayerCreated;
        public static event Action LocalPlayerDestroyed;
        
        [SerializeField]
        [Range(1, 5)]
        private int _movementRange = 2;
        
        public PlayerHealth Health { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        
        public bool IsLocalPlayer => this == LocalPlayer;
        public int MovementRange => _movementRange;
        public IDamageable Damageable => Health;


        private void Awake()
        {
            Health = GetComponent<PlayerHealth>();
        }


        public void SetGridPosition(Vector2Int gridPosition, Vector3 worldPosition)
        {
            GridPosition = gridPosition;
            transform.position = worldPosition;
        }
        
        
        public static void SetLocalPlayer(PlayerCharacter player)
        {
            LocalPlayer = player;
            LocalPlayerCreated?.Invoke(player);
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