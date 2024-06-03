using System.Collections;
using Cards;
using Entities.Players;
using UnityEngine;

namespace PhaseSystem.Players
{
    /// <summary>
    /// Allows the player to either play cards or move.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Player/Play Cards", fileName = "Phase_Players_Play_Cards", order = 0)]
    public class PlayerPlayCardsPhase : GamePhase
    {
        protected override IEnumerator OnExecute()
        {
            if (HasPlayerDoneAction())
                yield break;
            
            GameLoopManager.AllowCardPlay = true;
            
            // Yield until the player ends their turn.
            while (!GameLoopManager.IsEndTurnRequested() && !GameLoopManager.IsSkipPhaseRequested())
                yield return null;
            
            GameLoopManager.AllowCardPlay = false;
        }


        private static bool HasPlayerDoneAction() => PlayerCharacter.LocalPlayer.Movement.PlayerMovementsThisTurn > 0;
    }
}