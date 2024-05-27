using System.Collections;
using Entities.Enemies;
using UnityEngine;

namespace PhaseSystem.Enemies
{
    /// <summary>
    /// In this turn the enemies display their intentions to the player.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Enemies/Display Intentions", fileName = "Phase_Enemies_Intentions_Display", order = 0)]
    public class EnemyDisplayIntentionsPhase : GamePhase
    {
        public override string Name => "Display Intentions";
        
        
        protected sealed override IEnumerator OnExecute()
        {
            foreach (EnemyCharacter enemy in EnemyManager.Enemies)
            {
                enemy.UpdateAttackHighlighter();
            }
            yield return null;
        }
    }
}