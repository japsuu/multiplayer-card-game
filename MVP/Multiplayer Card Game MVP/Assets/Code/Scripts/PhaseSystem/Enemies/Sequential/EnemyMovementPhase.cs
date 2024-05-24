using System.Collections;
using Entities.Enemies;
using UnityEngine;

namespace PhaseSystem.Enemies.Sequential
{
    [CreateAssetMenu(menuName = "Phases/Enemies/Sequential/Move", fileName = "Move", order = 0)]
    public class EnemyMovementPhase : SequentialPhase<EnemyCharacter>
    {
        protected override IEnumerator OnExecute(EnemyCharacter instance)
        {
            yield return instance.Movement.Move();
        }
    }
}