using System;
using StateManagement.Turns;
using UnityHFSM;

namespace StateManagement
{
    /// <summary>
    /// A state machine that manipulates the global game state.
    /// </summary>
    public class GameStateMachine : StateMachine
    {
        public GameStateMachine()
        {
            // States
            AddState("PlayerTurn", new PlayerTurn());
            AddState("EnemyTurn", new EnemyTurn());
            
            // Transitions
            // Alternate between player and enemy turns
            AddTransition(new Transition("PlayerTurn", "EnemyTurn"));
            AddTransition(new Transition("EnemyTurn", "PlayerTurn"));
        }
        
        
        public void SkipCurrentState()
        {
            if (!GameState.AllowSkip)
                throw new InvalidOperationException("Skip button should not be interactable when skipping is not allowed.");
            
#warning TODO: Verify this works
            ActiveState.fsm.StateCanExit();
        }


        public void EndTurn()
        {
            if (!GameState.AllowEndTurn)
                throw new InvalidOperationException("End turn button should not be interactable when ending the turn is not allowed.");
#warning TODO: Implement EndTurn
            throw new NotImplementedException();
        }
    }
}