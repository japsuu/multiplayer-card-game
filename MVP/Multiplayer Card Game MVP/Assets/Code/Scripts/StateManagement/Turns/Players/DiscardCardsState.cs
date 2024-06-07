using UnityHFSM;

namespace StateManagement.Turns.Players
{
    public class DiscardCardsState : StateBase
    {
        public DiscardCardsState() : base(needsExitTime:true)
        {
        }


        public override void OnEnter()
        {
            GameState.SetShowHand(true);
            GameState.SetAllowCardDiscard(true);
        }


        public override void OnLogic()
        {
            // Exit if there are no cards to discard
            if (!GameState.HasHandCards)
                fsm.StateCanExit();
        }


#warning TODO: Move discard logic to discard state. 
        /*public override void OnLogic()
        {
            base.OnLogic();
            
            // Skipping
            /*if (GameState.IsSkipStateRequested)
            {
                fsm.StateCanExit();
                /*return;#2#
            }#1#
            
            /#1#/ Discarding
            if (GameState.HasHandCards && Input.GetKeyDown(KeyCode.D))
            {
                GameState.DiscardCard();

                // Exit if there are no more cards to discard
                if (!GameState.HasHandCards)
                    fsm.StateCanExit();
            }#1#
        }*/


        public override void OnExit()
        {
            GameState.SetAllowCardDiscard(false);
        }
    }
}