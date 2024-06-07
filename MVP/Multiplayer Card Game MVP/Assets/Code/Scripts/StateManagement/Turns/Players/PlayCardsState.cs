using UnityHFSM;

namespace StateManagement.Turns.Players
{
    public class PlayCardsState : StateBase
    {
        public PlayCardsState() : base(needsExitTime:true)
        {
        }


        public override void OnEnter()
        {
            GameState.SetAllowCardPlay(true);
        }


        public override void OnLogic()
        {
            // Exit if there are no cards to play
            if (!GameState.HasActivatedCards)
                fsm.StateCanExit();
        }


        /*public override void OnLogic()
        {
            base.OnLogic();
            
            // Skipping
            if (Input.GetKeyDown(KeyCode.S))
            {
                fsm.StateCanExit();
                return;
            }
            
            // Activating
            if (GameState.HasActivatedCards && Input.GetKeyDown(KeyCode.P))
            {
                GameState.PlayCard();

                // Exit if there are no more cards to play
                if (!GameState.HasActivatedCards)
                    fsm.StateCanExit();
            }
        }*/


        public override void OnExit()
        {
            GameState.SetAllowCardPlay(false);
        }
    }
}