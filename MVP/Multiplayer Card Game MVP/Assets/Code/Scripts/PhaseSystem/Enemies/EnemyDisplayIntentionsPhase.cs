using System.Collections;
using UnityEngine;

namespace PhaseSystem.Enemies
{
    /// <summary>
    /// In this turn the enemies display their intentions to the player.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Enemies/Display Intentions", fileName = "Phase_Enemies_Intentions_Display", order = 0)]
    public class EnemyDisplayIntentionsPhase : GamePhase
    {
        public override string Name => ">Display Intentions";
        
        
        public override IEnumerator Execute()
        {
            Debug.LogWarning("TODO: Implement displaying intentions to the player.");
            yield return null;
        }
    }
}