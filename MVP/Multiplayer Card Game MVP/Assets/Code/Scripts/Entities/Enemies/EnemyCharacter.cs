using System;
using Boards;
using DamageSystem;
using UnityEngine;

namespace Entities.Enemies
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EntityHealth))]
    public class EnemyCharacter : BoardEntity
    {
        private EnemyMovement _movement;


        protected override void Awake()
        {
            base.Awake();
            _movement = GetComponent<EnemyMovement>();
            _movement.Initialize(this);
        }


        private void OnEnable()
        {
            EnemyManager.AddEnemy(this);
        }
        
        
        private void OnDisable()
        {
            EnemyManager.RemoveEnemy(this);
        }
    }
    
    
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


        public void Move()
        {
            // Move to a random cell within the movement range, using BoardManager.Instance.MoveOccupant(occupant, cellPos);
            // Also make sure that the position is inside the board bounds.
            Vector2Int randomCell = BoardManager.Instance.GetRandomEmptyCell(_enemy.BoardPosition, _movementRange);
            BoardManager.Instance.MoveOccupant(_enemy, randomCell);
        }
    }
}