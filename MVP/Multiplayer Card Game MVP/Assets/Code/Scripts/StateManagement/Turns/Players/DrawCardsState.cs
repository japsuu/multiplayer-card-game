using System.Collections;
using Cards;
using UnityEngine;
using UnityHFSM;

namespace StateManagement.Turns.Players
{
    public class DrawCardsState : StateBase
    {
        public DrawCardsState() : base(needsExitTime:true)
        {
        }


        public override void OnEnter()
        {
            GameState.SetShowHand(true);
            GameManager.RunCoroutine(DrawCards());
        }


        private IEnumerator DrawCards()
        {
            yield return new WaitForSeconds(1f);
            
            // Draw cards until the player reaches their draw limit.
            while (PlayerHandManager.Instance.CanDrawCard)
            {
                yield return PlayerHandManager.Instance.DrawCard();
            }
            fsm.StateCanExit();
        }


        public override void OnExit()
        {
            GameState.SetShowHand(false);
        }
    }
}