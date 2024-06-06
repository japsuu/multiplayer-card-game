using UnityEngine;
using UnityHFSM;

namespace _Development
{
    public class EnemyTurn : HybridStateMachine
    {
        public EnemyTurn() : base(needsExitTime:true)
        {
            AddState("Wait", new State(
                    onLogic: state => {
                        if (state.timer.Elapsed > 2)
                            state.fsm.StateCanExit();
                    },
                    needsExitTime: true)
            );
            AddExitTransition(new Transition("Wait", ""));
        }


        public override void OnEnter()
        {
            Debug.Log("Enemy turn start");
            base.OnEnter();
        }


        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("Enemy turn end");
        }
    }
}