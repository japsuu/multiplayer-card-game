using UnityHFSM;

namespace StateManagement.Turns.Players
{
    public class PlayerTurnStartState : StateBase
    {
        public PlayerTurnStartState() : base(needsExitTime:true)
        {
            
        }


        public override void OnEnter()
        {
            GameManager.OnPlayerTurnStart();
            
            GameState.DisableEverything();
        }


        public override void OnLogic()
        {
            fsm.StateCanExit();
        }
    }
}