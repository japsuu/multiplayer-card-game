using System.Collections;
using Cards.AttackPatterns;
using UnityEngine;

namespace Cards.Data
{
    [CreateAssetMenu(menuName = "Cards/Create Skill Card", fileName = "Card_Skill_", order = 0)]
    public class SkillCardData : CardData
    {
        public override CellPattern CellPattern => null;
        
        
        public override IEnumerator ApplyBoardEffects(Vector2Int cell)
        {
            yield break;
        }
    }
}