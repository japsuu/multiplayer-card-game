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
        
        public void OnAddedToBoard(Vector2Int boardPos);
        public void OnMovedOnBoard(Vector2Int newBoardPos);
        public void OnRemovedFromBoard();
    }
}