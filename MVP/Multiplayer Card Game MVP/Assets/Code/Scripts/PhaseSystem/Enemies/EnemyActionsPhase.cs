using System.Collections;
using System.Collections.Generic;
using Entities.Enemies;
using UnityEngine;

namespace PhaseSystem.Enemies
{
    /// <summary>
    /// In this phase the enemies execute their actions.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Enemies/Execute Actions", fileName = "Phase_Enemies_Execute_Actions", order = 0)]
    public class EnemyActionsPhase : SequentialPhaseCollection<EnemyCharacter>
    {
        public override string Name => "Execute Enemy Actions";

        protected override IEnumerable<EnemyCharacter> Instances => EnemyManager.Enemies;
    }
}