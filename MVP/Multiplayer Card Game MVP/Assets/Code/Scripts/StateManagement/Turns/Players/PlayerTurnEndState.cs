using UnityHFSM;

namespace StateManagement.Turns.Players
{
    public class PlayerTurnEndState : StateBase
    {
        public PlayerTurnEndState() : base(needsExitTime:true, isGhostState:true)
        {
            
        }


        public override void OnEnter()
        {
            GameManager.OnPlayerTurnEnd();

            GameState.DisableEverything();
            
            // TODO: Check that all players are ready to end their turn
        }


        public override void OnLogic()
        {
            fsm.StateCanExit();
        }
    }
}