﻿using System.Collections;
using Cards;
using Entities.Players;
using UnityEngine;

namespace PhaseSystem.Players
{
    /// <summary>
    /// Allows the player to either play cards or move.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Player/Action Phase", fileName = "Phase_Players_Action", order = 0)]
    public class PlayerActionPhase : GamePhase
    {
        protected override IEnumerator OnEnter()
        {
            GameLoopManager.AllowCardDragging = true;
            return base.OnEnter();
        }
        
        
        protected override IEnumerator OnExecute()
        {
            // Since the player can either play cards or move, we wait until the player does one of these actions.
            // Once the player does one of these actions, we block the other action.
            // This is done by disabling the player's hand, or disabling the player's movement.
            
            // Yield until the player plays a card, moves, or ends their turn.
            while (!GameLoopManager.IsEndTurnRequested() && !GameLoopManager.IsSkipPhaseRequested() && !HasPlayerDoneAction())
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
            while (!GameLoopManager.IsEndTurnRequested() && !GameLoopManager.IsSkipPhaseRequested())
                yield return null;
        }


        protected override IEnumerator OnExit()
        {
            GameLoopManager.AllowCardDragging = false;
            return base.OnExit();
        }


        private static bool HasPlayerDoneAction() => PlayerHandManager.Instance.CardsPlayedThisTurn > 0 || PlayerCharacter.LocalPlayer.Movement.PlayerMovementsThisTurn > 0;
    }
}