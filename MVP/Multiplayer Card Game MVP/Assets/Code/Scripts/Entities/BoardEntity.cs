using System;
using System.Collections;
using Boards;
using DamageSystem;
using DG.Tweening;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(EntityHealth))]
    public abstract class BoardEntity : MonoBehaviour, ICellOccupant
    {
        public EntityHealth Health { get; private set; }
        public Vector2Int BoardPosition { get; private set; }
        
        public IDamageable Damageable => Health;
        
        
        public virtual IEnumerator OnAddedToBoard(Vector2Int boardPos)
        {
            yield return MoveTo(boardPos);
        }


        public virtual IEnumerator OnMovedOnBoard(Vector2Int newBoardPos)
        {
            yield return MoveTo(newBoardPos);
        }


        public virtual IEnumerator OnRemovedFromBoard()
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
            StartCoroutine(DeathCoroutine());
        }
        
        
        private IEnumerator DeathCoroutine()
        {
            yield return BoardManager.Instance.RemoveOccupant(this);
            
            Destroy(gameObject);
        }


        private IEnumerator MoveTo(Vector2Int boardPos)
        {
            BoardPosition = boardPos;
            
            if (!BoardManager.Instance.TryGetCellToWorld(boardPos, out Vector3 worldPos))
                throw new Exception("Could not get world position of cell.");
            
            yield return transform.DOMove(worldPos, 0.5f).WaitForCompletion();
        }
    }
}