using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityHFSM;

namespace _Development
{
    /// <summary>
    /// Player turn:
    ///     Draw cards up to your draw limit (5).
    ///     Decide if you want to move OR to play cards.
    /// 
    /// Moving
    ///     Click any of the highlighted cells near your player character, to move there.
    /// 
    /// Playing cards
    ///     Activate any number of cards from your hand.
    ///         Activating is done by moving the card to a card slot (3 slots per player).
    ///         If the slot already has a card, the existing card is removed to the discard pile.
    ///     Play any number of activated cards.
    ///         Play an activated card by clicking on it.
    ///         When a card is played, the actions defined on it are executed. The card stays in the card activation slot, but turns inactive and cannot be played anymore (until drawn again).
    /// </summary>
    public class TestingGameStateMachine : MonoBehaviour
    {
        private static TestingGameStateMachine instance;
        
        [SerializeField]
        [ReadOnly]
        private string _currentState;
        
        private StateMachine _fsm;


        private void Awake()
        {
            _fsm = new StateMachine();
            
            // ----- States -----
            _fsm.AddState("PlayerTurn", new PlayerTurn());
            _fsm.AddState("EnemyTurn", new EnemyTurn());
            
            // ----- Transitions -----
            _fsm.AddTransition(new Transition("PlayerTurn", "EnemyTurn"));
            _fsm.AddTransition(new Transition("EnemyTurn", "PlayerTurn"));
        }


        private void Start()
        {
            _fsm.Init();
        }


        private void Update()
        {
            _fsm.OnLogic();
            _currentState = _fsm.GetActiveHierarchyPath();
        }
    }
}