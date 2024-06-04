using System.Collections;
using Entities.Players;
using UnityEngine;

namespace PhaseSystem.Players
{
    /// <summary>
    /// Sets if the player is allowed to move.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Player/Set Allow Movement", fileName = "Phase_Players_Movement_SetAllow", order = 0)]
    public class PlayerSetAllowMovementPhase : GamePhase
    {
        [SerializeField]
        private bool _allowMovement = true;


        protected override IEnumerator OnExecute()
        {
            if (_allowMovement)
                PlayerCharacter.LocalPlayer.Movement.EnableMovement();
            else
                PlayerCharacter.LocalPlayer.Movement.DisableMovement();

            yield return null;
        }
    }
}