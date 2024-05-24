using System.Collections;
using Boards;
using UnityEngine;

namespace Entities.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 5)]
        private int _movementRange = 1;
        
        private EnemyCharacter _enemy;


        public void Initialize(EnemyCharacter enemy)
        {
            _enemy = enemy;
        }


        public IEnumerator Move()
        {
            // Move to a random cell within the movement range.
            Vector2Int randomCell = BoardManager.Instance.GetRandomEmptyCell(_enemy.BoardPosition, _movementRange, CellSide.Enemy);
            yield return BoardManager.Instance.MoveOccupant(_enemy, randomCell);
        }
    }
}