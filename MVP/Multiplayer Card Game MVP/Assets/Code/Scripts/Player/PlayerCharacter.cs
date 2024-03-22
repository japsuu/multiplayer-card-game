using UnityEngine;

namespace Player
{
    /// <summary>
    /// Represents a player character on the board.
    /// </summary>
    public class PlayerCharacter : MonoBehaviour
    {
        public static PlayerCharacter LocalPlayer { get; private set; }
        
        [SerializeField]
        [Range(1, 5)]
        private int _movementRange = 2;
        
        public Vector2Int GridPosition { get; private set; }
        public int MovementRange => _movementRange;
        
        
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