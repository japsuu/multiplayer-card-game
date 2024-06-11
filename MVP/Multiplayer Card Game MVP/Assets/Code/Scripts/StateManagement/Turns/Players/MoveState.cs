namespace StateManagement.Turns.Players
{
    public class MoveState : PlayerState
    {
        protected override bool AllowSkip => true;
        protected override bool AllowMovement => true;


        public override void OnLogic()
        {
#warning TODO: Move movement code to movement state.
            if (GameState.HasLocalPlayerMoved)
                fsm.StateCanExit();
        }
    }
}