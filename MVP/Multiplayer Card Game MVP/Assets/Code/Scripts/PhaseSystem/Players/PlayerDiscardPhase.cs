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
        protected override IEnumerator OnEnter()
        {
            PlayerHandManager.Instance.AllowDiscardCards(true);
            return base.OnEnter();
        }


        protected override IEnumerator OnExecute()
        {
            while (PlayerHandManager.Instance.CardCount > 0)
            {
                while (!GameLoopManager.IsEndTurnRequested() && !GameLoopManager.IsSkipPhaseRequested())
                    yield return null;
                yield break;
            }
        }
        
        
        protected override IEnumerator OnExit()
        {
            PlayerHandManager.Instance.AllowDiscardCards(false);
            return base.OnExit();
        }
    }
}