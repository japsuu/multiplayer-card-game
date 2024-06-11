using System.Collections;
using UI;
using UnityEngine;

namespace StateManagement.Turns.Players
{
    public class MoveState : PlayerState
    {
        protected override bool AllowSkip => true;
        protected override bool AllowMovement => true;


        protected override void OnEnterState()
        {
            GameManager.RunCoroutine(StartCoroutine());
        }


        private IEnumerator StartCoroutine()
        {
            yield return PhaseBanner.DisplayPhase("Movement", false);
            yield return new WaitUntil(() => GameState.HasLocalPlayerMoved);
            fsm.StateCanExit();
        }
    }
}