using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            List<BoardCell> availableCells = BoardManager.Instance.GetEmptyCells(_enemy.BoardPosition, _movementRange, CellSide.Enemy).ToList();
            if (availableCells.Count == 0)
            {
                Debug.LogWarning("No available cells to move to.");
                yield break;
            }
            
            BoardCell randomCell = availableCells[Random.Range(0, availableCells.Count)];
            
            yield return BoardManager.Instance.MoveOccupant(_enemy, randomCell.Position);
        }
    }
}