﻿using System;
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
        public static bool EndTurnRequested;
        
        public static event Action PlayerTurnStart;
        public static event Action PlayerTurnEnd;
        
        [SerializeField]
        private List<GamePhase> _phases;

        private int _currentPhase;
        private bool _shouldExecutePhases;


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
            EndTurnRequested = false;
            PlayerTurnStart?.Invoke();
        }
        
        
        public static void EndPlayerTurn()
        {
            EndTurnRequested = false;
            PlayerTurnEnd?.Invoke();
        }
        
        
        public static void RequestEndTurn()
        {
            EndTurnRequested = true;
            // print("End turn requested.");
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