using UnityEngine;

namespace Player
{
    /// <summary>
    /// Represents a player character on the board.
    /// </summary>
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerCharacter : MonoBehaviour
    {
        public static PlayerCharacter LocalPlayer { get; private set; }
        
        [SerializeField]
        [Range(1, 5)]
        private int _movementRange = 2;
        
        public PlayerHealth Health { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        
        public bool IsLocalPlayer => this == LocalPlayer;
        public int MovementRange => _movementRange;


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
        }
    }
}