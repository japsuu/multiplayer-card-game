using System;
using System.Collections;
using Boards;
using DamageNumbersPro;
using DamageSystem;
using DG.Tweening;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(EntityHealth))]
    public abstract class BoardEntity : MonoBehaviour, ICellOccupant
    {
        [SerializeField]
        private DamageNumber _damageNumberPrefab;
        
        [SerializeField]
        private DamageNumber _killedTextPrefab;
        
        public event Action Died;
        
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
            yield return null;
        }


        protected virtual void Awake()
        {
            Health = GetComponent<EntityHealth>();
            Health.Died += OnDied;
        }


        protected virtual void OnEnable()
        {
            Health.HealthChanged += OnHealthChanged;
        }


        protected virtual void OnDisable()
        {
            Health.HealthChanged -= OnHealthChanged;
        }


        private void OnHealthChanged(HealthChangedArgs args)
        {
            if (args.IsDamage)
                _damageNumberPrefab.Spawn(transform.position, args.HealthDifference.ToString());
        }


        protected virtual void OnDied()
        {
            StartCoroutine(DeathCoroutine());
        }
        
        
        private IEnumerator DeathCoroutine()
        {
            Died?.Invoke();
            
            _killedTextPrefab.Spawn(transform.position, "Dead!");
            
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