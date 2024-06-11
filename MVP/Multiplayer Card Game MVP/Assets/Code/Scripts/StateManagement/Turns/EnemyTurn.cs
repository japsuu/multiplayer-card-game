using System.Collections;
using Entities.Enemies;
using UnityEngine;
using UnityHFSM;

namespace StateManagement.Turns
{
    public class EnemyTurn : StateBase
    {
        public EnemyTurn() : base(needsExitTime:true)
        {
            
        }


        public override void OnEnter()
        {
            GameManager.RunCoroutine(ExecuteActions());
        }


        public override void OnExit()
        {
            
        }


        private IEnumerator ExecuteActions()
        {
            yield return new WaitForSeconds(1f);
            
            foreach (EnemyCharacter enemy in EnemyManager.Enemies)
            {
                enemy.UpdateAttackHighlighter();
                
                enemy.Attack.Execute(enemy.BoardPosition);
                enemy.ResetAttackHighlighter();
                yield return new WaitForSeconds(0.5f);
                
                yield return enemy.Movement.Move();
                
                enemy.UpdateAttackHighlighter();
            }
            
            fsm.StateCanExit();
        }
    }
}