using System.Collections;
using Cards;
using UnityEngine;

namespace PhaseSystem.Players
{
    /// <summary>
    /// Contains the logic for the players' turn.
    /// The logic is separated to multiple sub-phases.
    /// NOTE: Multiplayer related logic will be implemented later, so we can assume that there is only one player.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Players Phase", fileName = "Phase_Players", order = 0)]
    public class PlayersPhase : GamePhase
    {
        protected override IEnumerator OnExecute()
        {
            PlayerHandManager.Instance.ShowHand();
            
            yield return new WaitForSeconds(2f);
            
            // Since the player can either play cards or move, we wait until the player does one of these actions.
            // Once the player does one of these actions, we block the other action.
            // This is done by disabling the player's hand, or disabling the player's movement.
            
            // PlayerHandManager.Instance.HideHand();
            // yield return null;
        }
    }
}