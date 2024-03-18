using System.Collections;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/Create Skill Card", fileName = "Card_Skill_", order = 0)]
    public class SkillCardData : CardData
    {
        public override IEnumerator OnPlay(Vector2Int cell)
        {
            Debug.LogWarning("TODO: Implement card logic.");
            yield break;
        }
    }
}