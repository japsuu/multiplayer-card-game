using System.Collections;
using Cards;
using UnityEngine;

namespace StateManagement.Turns.Players
{
    public class DrawCardsState : PlayerState
    {
        protected override bool ShouldShowHand => true;


        protected override void OnEnterState()
        {
            GameManager.RunCoroutine(DrawCards());
        }


        private IEnumerator DrawCards()
        {
            yield return new WaitForSeconds(1f);

            // Draw cards until the player reaches their draw limit.
            while (PlayerHandManager.Instance.CanDrawCard)
                yield return PlayerHandManager.Instance.DrawCard();
            fsm.StateCanExit();
        }
    }
}