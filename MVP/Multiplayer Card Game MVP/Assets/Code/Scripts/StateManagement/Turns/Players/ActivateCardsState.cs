using System.Collections;
using UI;
using UnityEngine;

namespace StateManagement.Turns.Players
{
    public class ActivateCardsState : PlayerState
    {
        protected override bool ShouldShowHand => true;
        protected override bool AllowSkip => true;
        protected override bool AllowCardActivation => true;


        protected override void OnEnterState()
        {
            GameManager.RunCoroutine(StartCoroutine());
        }


        private IEnumerator StartCoroutine()
        {
            yield return PhaseBanner.DisplayPhase("Activate Cards", false);
            yield return new WaitUntil(() => !GameState.HasHandCards);
            fsm.StateCanExit();
        }
    }
}