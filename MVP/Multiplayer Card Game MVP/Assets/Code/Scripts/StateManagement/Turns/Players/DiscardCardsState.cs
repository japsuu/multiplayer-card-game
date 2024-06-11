using System.Collections;
using UI;
using UnityEngine;

namespace StateManagement.Turns.Players
{
    public class DiscardCardsState : PlayerState
    {
        protected override bool AllowSkip => true;
        protected override bool ShouldShowHand => true;
        protected override bool AllowCardDiscard => true;


        protected override void OnEnterState()
        {
            GameManager.RunCoroutine(StartCoroutine());
        }


        private IEnumerator StartCoroutine()
        {
            yield return PhaseBanner.DisplayPhase("Discard Cards", false);
            yield return new WaitUntil(() => !GameState.HasHandCards);
            fsm.StateCanExit();
        }
    }
}