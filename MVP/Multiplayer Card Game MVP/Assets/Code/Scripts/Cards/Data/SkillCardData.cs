using System.Collections;
using UnityEngine;

namespace Cards.Data
{
    [CreateAssetMenu(menuName = "Cards/Create Skill Card", fileName = "Card_Skill_", order = 0)]
    public class SkillCardData : CardData
    {
        protected override IEnumerator OnPlay(Vector2Int cell)
        {
            Debug.LogWarning("TODO: Implement card logic.");
            yield break;
        }


        public override void OnDrag(CardInstance draggedCard)
        {
            base.OnDrag(draggedCard);
            
            draggedCard.transform.position = (Vector2)Input.mousePosition;
        }


        public override void OnEndDrag(CardInstance draggedCard)
        {
            base.OnEndDrag(draggedCard);
            
            PlayerHandManager.Instance.PlayCard(draggedCard, Vector2Int.zero);
        }
    }
}