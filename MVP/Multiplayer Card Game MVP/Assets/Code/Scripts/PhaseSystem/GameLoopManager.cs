using System;
using System.Collections;
using System.Collections.Generic;
using Singletons;
using UnityEngine;

namespace PhaseSystem
{
    /// <summary>
    /// Like a state machine, but for game phases.
    /// Iterates through the phases one by one, until the game ends.
    /// </summary>
    public class GameLoopManager : SingletonBehaviour<GameLoopManager>
    {
        private static bool isSkipPhaseRequested;
        public static bool IsSkipPhaseRequested() => isSkipPhaseRequested;
        
        private static bool isEndTurnRequested;
        public static bool IsEndTurnRequested() => isEndTurnRequested;

        public static bool AllowCardDragging;
        
        public static event Action PlayerTurnStart;
        public static event Action PlayerTurnEnd;
        
        public static event Action<bool> RequestShowSkipButton;
        public static event Action<bool> RequestShowEndTurnButton;
        
        public static event Action<GamePhase> PhaseChange;
        
        [SerializeField]
        private List<GamePhase> _phases;

        private int _currentPhase;
        private bool _shouldExecutePhases;


        public static void SetShowSkipButton(bool show) => RequestShowSkipButton?.Invoke(show);
        public static void SetShowEndTurnButton(bool show) => RequestShowEndTurnButton?.Invoke(show);


        /// <summary>
        /// Starts iterating through the phases.
        /// Called when the game starts.
        /// </summary>
        public void StartGameLoop()
        {
            _currentPhase = 0;
            _shouldExecutePhases = true;
            StartCoroutine(ExecutePhases());
        }


        /// <summary>
        /// Stops iterating through the phases.
        /// Called when the player wins or loses.
        /// </summary>
        public void StopGameLoop()
        {
            _shouldExecutePhases = false;
        }
        
        
        public static void StartPlayerTurn()
        {
            isEndTurnRequested = false;
            isSkipPhaseRequested = false;
            PlayerTurnStart?.Invoke();
        }
        
        
        public static void EndPlayerTurn()
        {
            isEndTurnRequested = false;
            isSkipPhaseRequested = false;
            PlayerTurnEnd?.Invoke();
        }
        
        
        public static void RequestSkipPhase()
        {
            isSkipPhaseRequested = true;
        }
        
        
        public static void RequestEndTurn()
        {
            isEndTurnRequested = true;
        }
        
        
        public static void NotifyPhaseChange(GamePhase phase)
        {
            isSkipPhaseRequested = false;
            PhaseChange?.Invoke(phase);
        }
        
        
        private IEnumerator ExecutePhases()
        {
            while (_shouldExecutePhases)
            {
                if (_currentPhase >= _phases.Count)
                    _currentPhase = 0;

                GamePhase phase = _phases[_currentPhase];
                // Debug.Log($"Executing phase '{phase.Name}'...");
                
                yield return phase.Execute();
                
                _currentPhase++;
            }
        }
    }
}