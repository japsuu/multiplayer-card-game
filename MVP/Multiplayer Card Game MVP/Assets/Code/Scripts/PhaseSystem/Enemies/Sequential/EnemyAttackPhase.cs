using System.Collections;
using Entities.Enemies;
using UnityEngine;

namespace PhaseSystem.Enemies.Sequential
{
    [CreateAssetMenu(menuName = "Phases/Enemies/Sequential/Attack", fileName = "Attack", order = 0)]
    public class EnemyAttackPhase : SequentialPhase<EnemyCharacter>
    {
        protected override IEnumerator OnExecute(EnemyCharacter instance)
        {
            instance.Attack.Execute(instance.BoardPosition);
            instance.ResetAttackHighlighter();
            yield return new WaitForSeconds(1f);
        }
    }
}