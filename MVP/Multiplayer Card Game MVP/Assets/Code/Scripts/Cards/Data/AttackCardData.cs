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
        
        
        public override IEnumerator OnPlay(Vector2Int cell)
        {
            Debug.LogWarning("TODO: Implement card logic.");
            yield break;
        }


        public override void BuildHighlighter(CellHighlightGroup highlighterGroup)
        {
            base.BuildHighlighter(highlighterGroup);
            highlighterGroup.SetPulseSettings(CellHighlighter.PulseSettings.Attack);
            
            // Add highlighters for each cell in the attack pattern.
            foreach (Vector2Int relativePosition in _attackPattern.GetCells(Vector2Int.zero))
                highlighterGroup.AddRelativeHighlighter(relativePosition, Color.red);
        }
    }
}