using System.Collections;
using DamageSystem;
using JetBrains.Annotations;
using UnityEngine;

namespace Boards
{
    public interface ICellOccupant
    {
        [CanBeNull]
        public IDamageable Damageable { get; }
        
        public Vector2Int BoardPosition { get; }
        
        public IEnumerator OnAddedToBoard(Vector2Int boardPos);
        public IEnumerator OnMovedOnBoard(Vector2Int newBoardPos);
        public IEnumerator OnRemovedFromBoard();
    }
}