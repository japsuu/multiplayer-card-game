using UnityHFSM;

namespace StateManagement.Turns.Players
{
    public class MoveState : StateBase
    {
        public MoveState() : base(needsExitTime:true)
        {
        }


        public override void OnEnter()
        {
            GameState.SetAllowMovement(true);
        }
        
        
        public override void OnLogic()
        {
#warning TODO: Move movement code to movement state.
            if (GameState.HasLocalPlayerMoved)
                fsm.StateCanExit();
        }


        public override void OnExit()
        {
            GameState.SetAllowMovement(false);
        }
    }
}