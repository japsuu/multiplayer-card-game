using System;
using System.Collections;
using Boards;
using Utils.Singletons;

namespace StateManagement
{
    /// <summary>
    /// Updates and manages the <see cref="GameStateMachine"/>.
    /// </summary>
    public class GameManager : HiddenSingletonBehaviour<GameManager>
    {
        public static event Action PlayerTurnStart;
        public static event Action PlayerTurnEnd;
        public static event Action<bool> RequestShowSkipButton;
        public static event Action<bool> RequestShowEndTurnButton;
        
        private GameStateMachine _fsm;
        
        
        /// <summary>
        /// Called by the UI skip button.
        /// </summary>
        public static void SkipCurrentState() => Instance._fsm.SkipCurrentState();
        
        /// <summary>
        /// Called by the UI end turn button.
        /// </summary>
        public static void EndTurn() => Instance._fsm.EndTurn();
        
        public static void ShowSkipButton(bool show) => RequestShowSkipButton?.Invoke(show);
        public static void ShowEndTurnButton(bool show) => RequestShowEndTurnButton?.Invoke(show);
        
        public static void OnPlayerTurnStart() => PlayerTurnStart?.Invoke();
        public static void OnPlayerTurnEnd() => PlayerTurnEnd?.Invoke();
        public static void OnPlayerDeath() => throw new NotImplementedException();
        
        public static void RunCoroutine(IEnumerator coroutine) => Instance.StartCoroutine(coroutine);


        private void Awake()
        {
            _fsm = new GameStateMachine();
        }


        private void Start()
        {
            BoardManager.Instance.Initialize();
            
            _fsm.Init();
        }


        private void Update()
        {
            _fsm.OnLogic();
            string currentState = Instance._fsm.GetActiveHierarchyPath();
            
#if UNITY_EDITOR

            CurrentStateName = _fsm.GetActiveHierarchyPath();
        }
        public static string CurrentStateName;
#else
        }
#endif
    }
}