using Boards;
using DamageSystem;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EntityHealth))]
    public class EnemyCharacter : MonoBehaviour, ICellOccupant
    {
        private EntityHealth _health;
        
        public IDamageable Damageable => _health;
        public Vector2Int BoardPosition { get; private set; }


        public void OnAddedToBoard(Vector2Int boardPos)
        {
            throw new System.NotImplementedException();
        }


        public void OnMovedOnBoard(Vector2Int newBoardPos)
        {
            throw new System.NotImplementedException();
        }


        public void OnRemovedFromBoard()
        {
            throw new System.NotImplementedException();
        }


        private void Awake()
        {
            _health = GetComponent<EntityHealth>();
        }
    }
}