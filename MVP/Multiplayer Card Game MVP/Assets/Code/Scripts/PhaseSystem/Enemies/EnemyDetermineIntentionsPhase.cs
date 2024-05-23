﻿using System.Collections;
using UnityEngine;

namespace PhaseSystem.Enemies
{
    /// <summary>
    /// In this phase the enemies determine their intentions (what they want to do next).
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Enemies/Determine Intentions", fileName = "Phase_Enemies_Intentions_Determine", order = 0)]
    public class EnemyDetermineIntentionsPhase : GamePhase
    {
        public override string Name => "Determine Intentions";
        
        
        public override IEnumerator Execute()
        {
            Debug.LogWarning("TODO: Implement determining enemy intentions.");
            yield return null;
        }
    }
}