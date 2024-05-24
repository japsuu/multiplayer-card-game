using System.Collections;
using System.Collections.Generic;
using DamageSystem;
using PhaseSystem;
using UnityEngine;

namespace Entities.Enemies
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EntityHealth))]
    public class EnemyCharacter : BoardEntity
    {
        [SerializeField]
        private List<SequentialPhase<EnemyCharacter>> _actions;
        
        public EnemyMovement Movement { get; private set; }


        public IEnumerator ExecuteActions()
        {
            foreach (SequentialPhase<EnemyCharacter> action in _actions)
            {
                yield return action.Execute(this);
            }
        }


        protected override void Awake()
        {
            base.Awake();
            Movement = GetComponent<EnemyMovement>();
            Movement.Initialize(this);
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
}