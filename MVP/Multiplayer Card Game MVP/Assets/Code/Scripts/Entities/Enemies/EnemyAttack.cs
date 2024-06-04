using System;
using System.Collections.Generic;
using Boards;
using UnityEngine;

namespace Entities.Enemies
{
    [Serializable]
    public class EnemyAttack
    {
        [SerializeField]
        private int _damage = 20;
        
        [NonSerialized]
        private EnemyCharacter _host;

        
        public IEnumerable<Vector2Int> GetAffectedCellPositions(Vector2Int origin)
        {
            // Traverse cells downwards from the origin, stopping at the first cell that is not empty.
            foreach (BoardCell cell in BoardManager.Instance.TraverseCells(origin, GridDirection.Down))
            {
                yield return cell.Position;
                
                if (cell.IsOccupied)
                    yield break;
            }
        }
        
        
        public void Execute(Vector2Int origin)
        {
            foreach (Vector2Int damagePos in GetAffectedCellPositions(origin))
            {
                if (BoardManager.Instance.TryGetCell(damagePos, out BoardCell damagedCell))
                {
                    damagedCell.Occupant?.Damageable?.TakeDamage(_damage, _host);
                }
            }
        }


        public void Initialize(EnemyCharacter host)
        {
            _host = host;
        }
    }
}