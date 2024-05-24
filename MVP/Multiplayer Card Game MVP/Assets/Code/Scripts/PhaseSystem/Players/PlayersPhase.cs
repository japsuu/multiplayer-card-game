using System.Collections;
using Cards;
using Player;
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
            GameLoopManager.StartPlayerTurn();
            
            // Enable all actions.
            PlayerHandManager.Instance.ShowHand();
            PlayerCharacter.LocalPlayer.Movement.EnableMovement();
            
            // Since the player can either play cards or move, we wait until the player does one of these actions.
            // Once the player does one of these actions, we block the other action.
            // This is done by disabling the player's hand, or disabling the player's movement.
            
            // Yield until the player plays a card, moves, or ends their turn.
            while (!IsTurnEndRequested() && !HasPlayerDoneAction())
                yield return null;
            
            if (PlayerCharacter.LocalPlayer.Movement.PlayerMovementsThisTurn > 0)
            {
                // The player moved, so we disable the hand.
                PlayerHandManager.Instance.HideHand();
            }
            else
            {
                // The player played a card, so we disable the movement.
                PlayerCharacter.LocalPlayer.Movement.DisableMovement();
            }

            // Yield until the player ends their turn.
            yield return new WaitUntil(IsTurnEndRequested);
            
            // Disable all actions.
            PlayerHandManager.Instance.HideHand();
            PlayerCharacter.LocalPlayer.Movement.DisableMovement();
            
            GameLoopManager.EndPlayerTurn();
        }

        /*private static IEnumerator WaitUntilPlayerEndsTurn()
        {
            bool trigger = false;
            Action triggerAction = () => trigger = true;
            GameLoopManager.RequestEndPlayerTurn += triggerAction;
            yield return new WaitUntil(() => trigger);
            GameLoopManager.RequestEndPlayerTurn -= triggerAction;
        }*/


        private static bool IsTurnEndRequested() => GameLoopManager.EndTurnRequested;
        private static bool HasPlayerDoneAction() => PlayerHandManager.Instance.CardsPlayedThisTurn > 0 || PlayerCharacter.LocalPlayer.Movement.PlayerMovementsThisTurn > 0;
    }
}