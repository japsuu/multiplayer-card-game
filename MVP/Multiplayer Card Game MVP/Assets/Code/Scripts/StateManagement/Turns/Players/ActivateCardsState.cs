namespace StateManagement.Turns.Players
{
    public class ActivateCardsState : PlayerState
    {
        protected override bool ShouldShowHand => true;
        protected override bool AllowSkip => true;
        protected override bool AllowCardActivation => true;


        public override void OnLogic()
        {
            // Exit if there are no cards to activate
            if (!GameState.HasHandCards)
                fsm.StateCanExit();
        }
    }
}