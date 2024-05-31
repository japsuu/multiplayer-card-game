using System.Collections;
using Cards;
using UnityEngine;

namespace PhaseSystem.Players
{
    /// <summary>
    /// Allows the player to discard any number of cards from their hand.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Player/Discard Cards", fileName = "Phase_Players_Discard", order = 0)]
    public class PlayerDiscardPhase : GamePhase
    {
        protected override IEnumerator OnExecute()
        {
            // Draw cards until the player reaches their draw limit.
            while (PlayerHandManager.Instance.CardCount > 0)
            {
                while (!GameLoopManager.IsEndTurnRequested() && !GameLoopManager.IsSkipPhaseRequested())
                    yield return null;
                yield break;
#warning TODO: Implement the player's ability to discard cards.
            }
        }
    }
}