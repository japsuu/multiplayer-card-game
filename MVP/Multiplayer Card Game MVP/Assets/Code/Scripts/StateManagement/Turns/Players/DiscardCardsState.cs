namespace StateManagement.Turns.Players
{
    public class DiscardCardsState : PlayerState
    {
        protected override bool AllowSkip => true;
        protected override bool ShouldShowHand => true;
        protected override bool AllowCardDiscard => true;


        public override void OnLogic()
        {
#warning TODO: Move discard logic to discard state.

            // Exit if there are no cards to discard
            if (!GameState.HasHandCards)
                fsm.StateCanExit();
        }
    }
}