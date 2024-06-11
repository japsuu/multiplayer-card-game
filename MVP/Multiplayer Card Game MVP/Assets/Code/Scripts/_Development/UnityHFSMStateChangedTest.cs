using UnityEngine;
using UnityHFSM;

namespace _Development
{
    /// <summary>
    /// This example demonstrates how the <see cref="StateMachine.StateChanged"/> event does NOT get triggered when a nested state machine changes its state.
    /// </summary>
    public class UnityHFSMStateChangedTest : MonoBehaviour
    {
        private StateMachine _rootFsm;


        private void Awake()
        {
            _rootFsm = new StateMachine();
            
            // ----- Root States -----
            // State A: Normal state that waits for one second before transitioning to the next state.
            _rootFsm.AddState("State A",
                onEnter: _ =>
                {
                    print("Enter state A");
                },
                onLogic: state =>
                {
                    if (state.timer.Elapsed > 1)
                        state.fsm.StateCanExit();
                },
                needsExitTime: true
            );
            
            // State B: A state machine, that contains two states (X and Y).
            StateMachine stateBFsm = new(needsExitTime: true);
            stateBFsm.AddState("Nested X",
                onEnter: _ =>
                {
                    print("Enter state B-X");
                },
                onLogic: state =>
                {
                    if (state.timer.Elapsed > 1)
                        state.fsm.StateCanExit();
                },
                needsExitTime: true
            );
            stateBFsm.AddState("Nested Y",
                onEnter: _ =>
                {
                    print("Enter state B-Y");
                },
                onLogic: state =>
                {
                    if (state.timer.Elapsed > 1)
                        state.fsm.StateCanExit();
                },
                needsExitTime: true
            );
            // Add state B transitions. Nested Y is an exit transition.
            stateBFsm.AddTransition(new Transition("Nested X", "Nested Y"));
            stateBFsm.AddExitTransition(new Transition("Nested Y", ""));
            // Add the state machine to the root FSM.
            _rootFsm.AddState("State B", stateBFsm);
            
            
            // ----- Root Transitions -----
            // Alternate between "state A" and "state B".
            _rootFsm.AddTransition(new Transition("State A", "State B"));
            _rootFsm.AddTransition(new Transition("State B", "State A"));
        }
        
        private void OnEnable() => _rootFsm.StateChanged += OnStateChanged;
        private void OnDisable() => _rootFsm.StateChanged -= OnStateChanged;
        
        private void Start() => _rootFsm.Init();
        private void Update() => _rootFsm.OnLogic();
        
        private void OnStateChanged(StateBase<string> newState)
        {
            print($"StateChanged @: {newState.name}");
        }
    }
}