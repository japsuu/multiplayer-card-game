using System.Collections;
using UnityEngine;

namespace Cards.Data
{
    [CreateAssetMenu(menuName = "Cards/Create Skill Card", fileName = "Card_Skill_", order = 0)]
    public class SkillCardData : CardData
    {
        public override void OnDrag(CardInstance draggedCard)
        {
            base.OnDrag(draggedCard);
            
            draggedCard.transform.position = (Vector2)Input.mousePosition;
        }


        public override void OnEndDrag(CardInstance draggedCard, bool shouldPlayCard)
        {
            base.OnEndDrag(draggedCard, shouldPlayCard);
            
            if (shouldPlayCard)
            {
                PlayerHandManager.Instance.PlayCard(draggedCard, Play());
            }
        }
        
        
        private IEnumerator Play()
        {
            Debug.LogWarning("TODO: Implement card logic.");
            yield break;
        }
    }
}