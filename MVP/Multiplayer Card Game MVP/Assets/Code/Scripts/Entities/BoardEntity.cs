using System;
using Boards;
using DamageSystem;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(EntityHealth))]
    public abstract class BoardEntity : MonoBehaviour, ICellOccupant
    {
        public EntityHealth Health { get; private set; }
        public Vector2Int BoardPosition { get; private set; }
        
        public IDamageable Damageable => Health;
        
        
        public virtual void OnAddedToBoard(Vector2Int boardPos)
        {
            MoveTo(boardPos);
        }


        public virtual void OnMovedOnBoard(Vector2Int newBoardPos)
        {
            MoveTo(newBoardPos);
        }


        public virtual void OnRemovedFromBoard()
        {
            throw new NotImplementedException();
        }


        protected virtual void Awake()
        {
            Health = GetComponent<EntityHealth>();
            Health.Died += OnDied;
        }


        protected virtual void OnDied()
        {
            BoardManager.Instance.RemoveOccupant(this);
            
            Destroy(gameObject);
        }


        private void MoveTo(Vector2Int boardPos)
        {
            BoardPosition = boardPos;
            
            if (!BoardManager.Instance.TryGetCellToWorld(boardPos, out Vector3 worldPos))
                throw new Exception("Could not get world position of cell.");
            
            transform.position = worldPos;
        }
    }
}