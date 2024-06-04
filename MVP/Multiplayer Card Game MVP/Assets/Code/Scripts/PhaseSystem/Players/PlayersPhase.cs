using System.Collections;
using UnityEngine;

namespace PhaseSystem.Players
{
    /// <summary>
    /// Contains the logic for the players' turn.
    /// The logic is separated to multiple sub-phases.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Players Phase", fileName = "Phase_Players", order = 0)]
    public class PlayersPhase : GamePhaseCollection
    {
        protected override IEnumerator OnEnter()
        {
            GameLoopManager.StartPlayerTurn();
            yield return null;
        }


        protected override IEnumerator OnExit()
        {
            // Yield until the player ends their turn.
            yield return new WaitUntil(GameLoopManager.IsEndTurnRequested);
            
            GameLoopManager.EndPlayerTurn();
        }
    }
}