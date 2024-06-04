using Cards.AttackPatterns;
using UnityEngine;

namespace Cards.Data
{
    /// <summary>
    /// Basic card that has no effect when played.
    /// May still have tags that trigger on certain events.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Create Skill Card", fileName = "Card_Skill_", order = 0)]
    public class SkillCard : CardData
    {
        public override CellPattern CellPattern => null;
    }
}