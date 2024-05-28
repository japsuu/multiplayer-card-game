using System.Collections;
using Boards;
using Entities.Players;
using UnityEngine;

namespace Entities.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 5)]
        private int _movementActions = 2;
        
        private EnemyCharacter _enemy;
        // Used to alternate between moving up and down when blocked by obstacles. Changes when either the top or bottom of the board is reached. -1 or 1.
        private int _verticalMovementMultiplier = 1;

        private static readonly Vector2Int[] PrioritizedDirectionsLeft =
        {
            new(-1, 0),
            new(-1, 1), // Y might be multiplied with -1 to move down instead of up.
            new(0, 1),  // Y might be multiplied with -1 to move down instead of up.
        };
        
        private static readonly Vector2Int[] PrioritizedDirectionsRight =
        {
            new(1, 0),
            new(1, 1),  // Y might be multiplied with -1 to move down instead of up.
            new(0, 1),  // Y might be multiplied with -1 to move down instead of up.
        };


        public void Initialize(EnemyCharacter enemy)
        {
            _enemy = enemy;
        }


        public IEnumerator Move()
        {
            int executedMovements = 0;
            
            int closestPlayerX = PlayerManager.FindNearestHorizontalPlayer(_enemy.BoardPosition).BoardPosition.x;
            
            // Determine whether the enemy should move left or right to reach the closest player.
            Vector2Int[] prioritizedDirections = _enemy.BoardPosition.x < closestPlayerX ? PrioritizedDirectionsRight : PrioritizedDirectionsLeft;

            // While movements available and cannot attack the player, move towards the player.
            while (executedMovements < _movementActions && _enemy.BoardPosition.x != closestPlayerX)
            {
                // Check all directions based on their priority, and assign a target position if a valid one is found.
                Vector2Int? targetPos = null;
                foreach (Vector2Int dir in prioritizedDirections)
                {
                    Vector2Int direction = new(dir.x, dir.y * _verticalMovementMultiplier);
                
                    bool outOfBounds = !BoardManager.Instance.TryGetCell(_enemy.BoardPosition + direction, out BoardCell cell);
                    if (outOfBounds)
                    {
                        // Flip the vertical movement multiplier to move in the opposite vertical direction.
                        _verticalMovementMultiplier *= -1;
                        direction = new Vector2Int(dir.x, dir.y * _verticalMovementMultiplier);
                    }
                    
                    // If the cell is occupied, skip this direction.
                    if (cell.Occupant != null)
                        continue;
                
                    if (Globals.VerboseEnemyMovement)
                        print($"Moving towards {direction} from {_enemy.BoardPosition}.");
                
                    // Since boards are rectangles and the vertical movement direction is now flipped, we can assume that the cell is not out of bounds.
                    targetPos = _enemy.BoardPosition + direction;
                    break;
                }
                
                if (targetPos == null)
                {
                    if (Globals.VerboseEnemyMovement)
                        print($"No available directions to move to from {_enemy.BoardPosition}.");
                    
                    break;
                }
                yield return BoardManager.Instance.MoveOccupant(_enemy, targetPos.Value);
                executedMovements++;
            }
        }
    }
}