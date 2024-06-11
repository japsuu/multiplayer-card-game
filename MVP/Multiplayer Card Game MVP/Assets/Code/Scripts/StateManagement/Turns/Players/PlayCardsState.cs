using System.Collections;
using UI;
using UnityEngine;

namespace StateManagement.Turns.Players
{
    public class PlayCardsState : PlayerState
    {
        protected override bool AllowSkip => true;
        protected override bool AllowCardPlay => true;


        protected override void OnEnterState()
        {
            GameManager.RunCoroutine(StartCoroutine());
        }


        private IEnumerator StartCoroutine()
        {
            yield return PhaseBanner.DisplayPhase("Play Cards", false);
            yield return new WaitUntil(() => !GameState.HasActivatedCards);
            fsm.StateCanExit();
        }
    }
}