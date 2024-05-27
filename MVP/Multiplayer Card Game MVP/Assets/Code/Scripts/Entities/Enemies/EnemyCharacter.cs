using System;
using System.Collections;
using System.Collections.Generic;
using Boards;
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
        
        [SerializeField]
        private EnemyAttack _attack;

        private CellHighlightGroup _highlighterGroup;
        
        public EnemyMovement Movement { get; private set; }
        
        public EnemyAttack Attack => _attack;


        public IEnumerator ExecuteActions()
        {
            foreach (SequentialPhase<EnemyCharacter> action in _actions)
            {
                yield return action.Execute(this);
            }
        }
        
        
        public void UpdateAttackHighlighter()
        {
            ResetAttackHighlighter();
            
            foreach (Vector2Int cell in _attack.GetAffectedCellPositions(BoardPosition))
                _highlighterGroup.AddHighlighter(cell, Color.yellow);
        }
        
        
        public void ResetAttackHighlighter()
        {
            _highlighterGroup.ResetHighlighters();
        }


        protected override void Awake()
        {
            base.Awake();
            Movement = GetComponent<EnemyMovement>();
            Movement.Initialize(this);
        }


        private void Start()
        {
            _highlighterGroup = BoardManager.Instance.CreateHighlightGroup();
            _highlighterGroup.SetPulseSettings(CellHighlighter.PulseSettings.Attack);
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