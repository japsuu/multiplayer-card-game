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
        private readonly PlayerTurn _playerTurn;
        private readonly EnemyTurn _enemyTurn;
        
        
        public GameStateMachine()
        {
            // States
            _playerTurn = new PlayerTurn();
            _enemyTurn = new EnemyTurn();
            AddState("PlayerTurn", _playerTurn);
            AddState("EnemyTurn", _enemyTurn);
            
            // Transitions
            // Alternate between player and enemy turns
            AddTransition(new Transition("PlayerTurn", "EnemyTurn"));
            AddTransition(new Transition("EnemyTurn", "PlayerTurn"));
        }
        
        
        public void SkipCurrentState()
        {
            if (!GameState.AllowSkip)
                throw new InvalidOperationException("Skip button should not be interactable when skipping is not allowed.");
            
            if (ActiveState != _playerTurn)
                throw new InvalidOperationException("Skipping is only allowed during the player's turn.");
            
            _playerTurn.SkipCurrentState();
        }


        public void EndTurn()
        {
            if (!GameState.AllowEndTurn)
                throw new InvalidOperationException("End turn button should not be interactable when ending the turn is not allowed.");
            
            if (ActiveState != _playerTurn)
                throw new InvalidOperationException("Ending the turn is only allowed during the player's turn.");
            
            _playerTurn.EndTurn();
        }
    }
}