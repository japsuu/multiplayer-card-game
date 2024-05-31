using Boards;
using DamageSystem;
using UnityEngine;

namespace Entities.Enemies
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EntityHealth))]
    public class EnemyCharacter : BoardEntity
    {
        [SerializeField]
        private EnemyAttack _attack;

        private CellHighlightGroup _highlighterGroup;
        
        public EnemyMovement Movement { get; private set; }
        
        public EnemyAttack Attack => _attack;
        
        
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


        protected override void OnEnable()
        {
            base.OnEnable();
            EnemyManager.AddEnemy(this);
            BoardManager.BoardUpdated += UpdateAttackHighlighter;
        }
        
        
        protected override void OnDisable()
        {
            base.OnDisable();
            EnemyManager.RemoveEnemy(this);
            BoardManager.BoardUpdated -= UpdateAttackHighlighter;
        }
    }
}