using UnityEngine;
using UnityHFSM;

namespace StateManagement.Turns.Players
{
    public class AskMoveOrPlayCardsState : StateBase
    {
        public AskMoveOrPlayCardsState() : base(needsExitTime:true)
        {
        }

        
        public override void OnEnter()
        {
            GameState.SetAllowEndTurn(true);
        }
        
        
        public override void OnLogic()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                GameState.SetSelectedPlayerAction(PlayerAction.Move);
                fsm.StateCanExit();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                GameState.SetSelectedPlayerAction(PlayerAction.PlayCards);
                fsm.StateCanExit();
            }
        }
    }
}