namespace StateManagement.Turns.Players
{
    public class PlayerTurnStartState : PlayerState
    {
        protected override bool ShouldShowHand => false;
        protected override bool AllowMovement => false;
        protected override bool AllowCardDiscard => false;
        protected override bool AllowCardActivation => false;
        protected override bool AllowCardPlay => false;
        protected override bool AllowSkip => false;
        protected override bool AllowEndTurn => false;


        protected override void OnEnterState()
        {
            GameManager.OnPlayerTurnStart();

            GameState.SetSelectedPlayerAction(PlayerAction.None);
        }


        public override void OnLogic()
        {
            fsm.StateCanExit();
        }
    }
}