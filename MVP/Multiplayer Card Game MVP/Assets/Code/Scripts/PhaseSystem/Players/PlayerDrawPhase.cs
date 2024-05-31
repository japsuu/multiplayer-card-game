using System.Collections;
using Cards;
using UnityEngine;

namespace PhaseSystem.Players
{
    /// <summary>
    /// Draws cards for the player until the player reaches their card draw limit.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Player/Draw Cards To Limit", fileName = "Phase_Players_Draw", order = 0)]
    public class PlayerDrawPhase : GamePhase
    {
        protected override IEnumerator OnExecute()
        {
            // Draw cards until the player reaches their draw limit.
            while (PlayerHandManager.Instance.CardCount < PlayerHandManager.Instance.DrawLimit)
            {
                PlayerHandManager.Instance.AddCardToHand(PlayerHandManager.Instance.RemoveCardFromDrawPile());
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}