namespace StateManagement.Turns.Players
{
    public class PlayCardsState : PlayerState
    {
        protected override bool AllowSkip => true;
        protected override bool AllowCardPlay => true;


        public override void OnLogic()
        {
            // Exit if there are no cards to play
            if (!GameState.HasActivatedCards)
                fsm.StateCanExit();
        }
    }
}