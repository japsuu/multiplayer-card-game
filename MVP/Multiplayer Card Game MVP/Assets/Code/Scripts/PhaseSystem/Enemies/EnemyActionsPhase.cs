using System.Collections;
using Entities.Enemies;
using UnityEngine;

namespace PhaseSystem.Enemies
{
    /// <summary>
    /// In this phase the enemies execute their actions.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Enemies/Execute Actions", fileName = "Phase_Enemies_Execute_Actions", order = 0)]
    public class EnemyActionsPhase : GamePhase /*SequentialPhaseCollection<EnemyCharacter>*/
    {
        public override string Name => "Execute Enemy Actions";


        protected sealed override IEnumerator OnExecute()
        {
            // Execute all enemy actions sequentially.
            foreach (EnemyCharacter enemy in EnemyManager.Enemies)
            {
                yield return enemy.ExecuteActions();
            }
        }
    }
}