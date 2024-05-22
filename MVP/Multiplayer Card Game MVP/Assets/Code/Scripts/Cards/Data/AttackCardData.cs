using System.Collections;
using Boards;
using Cards.AttackPatterns;
using UnityEngine;

namespace Cards.Data
{
    [CreateAssetMenu(menuName = "Cards/Create Attack Card", fileName = "Card_Attack_", order = 0)]
    public class AttackCardData : CardData
    {
        [SerializeField]
        private GridPattern _attackPattern;


        public override void OnStartDrag(CardInstance draggedCard)
        {
            base.OnStartDrag(draggedCard);
            
            // Create a highlighter group for the attack pattern of this card.
            CellHighlightGroup highlighterGroup = BoardManager.Instance.CreateHighlightGroup();
            highlighterGroup.SetPulseSettings(CellHighlighter.PulseSettings.Attack);
            
            // Add highlighters for each cell in the attack pattern.
            foreach (Vector2Int relativePosition in _attackPattern.GetCells(Vector2Int.zero))
                highlighterGroup.AddRelativeHighlighter(relativePosition, Color.red);
            
            // Assign the highlighter group to the dragged card.
            draggedCard.Highlighter = highlighterGroup;
            
            TargetingArrow.Instance.Activate(draggedCard.transform);
        }


        public override void OnDrag(CardInstance draggedCard)
        {
            base.OnDrag(draggedCard);
            
            CellHighlightGroup highlighter = draggedCard.Highlighter;
            
            if (TargetingArrow.Instance.TryGetCell(out Vector2Int cell))
            {
                BoardManager.Instance.TryGetCellToWorld(cell, out Vector3 pos);
                highlighter.transform.position = pos;
                highlighter.gameObject.SetActive(true);
            }
            else
            {
                highlighter.gameObject.SetActive(false);
            }
        }


        public override void OnEndDrag(CardInstance draggedCard)
        {
            base.OnEndDrag(draggedCard);
            
            if (TargetingArrow.Instance.TryGetCell(out Vector2Int cell))
            {
                PlayerHandManager.Instance.PlayCard(draggedCard, cell);
            }
            
            TargetingArrow.Instance.Deactivate();
        }
        
        
        protected override IEnumerator OnPlay(Vector2Int cell)
        {
            Debug.LogWarning("TODO: Implement card logic.");
            yield break;
        }
    }
}