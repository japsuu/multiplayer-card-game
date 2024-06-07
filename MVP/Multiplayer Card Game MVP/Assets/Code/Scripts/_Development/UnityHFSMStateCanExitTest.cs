using UnityEngine;
using UnityHFSM;

namespace _Development
{
    public class TestState : StateBase
    {
        public TestState() : base(needsExitTime:true) { }


        public override void OnEnter()
        {
            Debug.Log("TestState.OnEnter");
            
            // Calling "fsm.StateCanExit" in OnEnter does not allow the state to exit.
            fsm.StateCanExit();
            Debug.Log("TestState.OnEnter: StateCanExit called");
        }


        public override void OnLogic()
        {
            Debug.Log("TestState.OnLogic");
        }
    }
    
    /// <summary>
    /// This example demonstrates how the calling "StateCanExit" in OnEnter does not allow the state to exit.
    /// </summary>
    public class UnityHFSMStateCanExitTest : MonoBehaviour
    {
        private StateMachine _rootFsm;


        private void Awake()
        {
            _rootFsm = new StateMachine();
            
            // ----- States -----
            _rootFsm.AddState("StateA",
                onEnter: _ =>
                {
                    print("StateA.OnEnter");
                },
                onLogic: state =>
                {
                    if (state.timer.Elapsed > 1)
                        state.fsm.StateCanExit();
                },
                needsExitTime: true
            );
            
            _rootFsm.AddState("TestState", new TestState());
            
            // ----- Transitions -----
            // Alternate between "state A" and "TestState".
            _rootFsm.AddTransition(new Transition("StateA", "TestState"));
            _rootFsm.AddTransition(new Transition("TestState", "StateA"));
        }
        
        private void Start() => _rootFsm.Init();
        private void Update() => _rootFsm.OnLogic();
    }
}