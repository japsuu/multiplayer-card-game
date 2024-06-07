using UnityHFSM;

namespace StateManagement.Turns.Players
{
    public class ActivateCardsState : StateBase
    {
        public ActivateCardsState() : base(needsExitTime:true)
        {
        }


        public override void OnEnter()
        {
            GameState.SetAllowCardActivation(true);
        }


        public override void OnLogic()
        {
            // Exit if there are no cards to activate
            if (!GameState.HasHandCards)
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
            if (GameState.HasHandCards && Input.GetKeyDown(KeyCode.A))
            {
                GameState.ActivateCard();

                // Exit if there are no more cards to activate
                if (!GameState.HasHandCards)
                    fsm.StateCanExit();
            }
        }*/


        public override void OnExit()
        {
            GameState.SetAllowCardActivation(false);
        }
    }
}