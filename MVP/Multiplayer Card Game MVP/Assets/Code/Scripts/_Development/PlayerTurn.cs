using UnityEngine;
using UnityHFSM;

namespace _Development
{
    public class PlayerTurn : HybridStateMachine
    {
        public PlayerTurn() : base(needsExitTime:true)
        {
            // ----- States -----
            AddState("DrawCards", new DrawCardsState());
            AddState("AskMoveOrPlayCards", new AskMoveOrPlayCardsState());
            AddState("Move", new MoveState());      // Exit only by ending turn
            AddState("DiscardCards", new DiscardCardsState());
            AddState("ActivateCards", new ActivateCardsState());
            AddState("PlayCards", new PlayCardsState());
            //TODO: WaitForAllReady state
            
            // ----- Transitions -----
            AddTransition(new Transition("DrawCards", "AskMoveOrPlayCards"));
            AddTriggerTransition("OnPlayerSelectMove", new Transition("AskMoveOrPlayCards", "Move"));
            AddTriggerTransition("OnPlayerSelectPlayCards", new Transition("AskMoveOrPlayCards", "DiscardCards"));
            AddTransition(new Transition("DiscardCards", "ActivateCards", _ => TestingGameState.HasHandCards));  // Only allow activating cards if there are cards in hand
            AddTransition(new Transition("DiscardCards", "PlayCards"));
            AddTransition(new Transition("ActivateCards", "PlayCards", _ => TestingGameState.HasActivatedCards));  // Only allow playing cards if there are activated cards
            
            // Exit transitions
            AddExitTransition(new Transition("Move", ""));
            AddExitTransition(new Transition("PlayCards", ""));
        }


        public override void OnEnter()
        {
            base.OnEnter();
        }


        public override void OnLogic()
        {
            base.OnLogic();
            
            if (TestingGameState.AllowEndTurn && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Player requesting end turn");
                RequestExit();
                TestingGameState.SetAllowEndTurn(false);
            }
        }
    }
    
    
    public class DrawCardsState : StateBase
    {
        private readonly ITimer _timer;
        
        
        public DrawCardsState() : base(needsExitTime:true)
        {
            _timer = new Timer();
        }


        public override void OnEnter()
        {
            _timer.Reset();
            
            Debug.Log("Drawing cards...");
            TestingGameState.SetCardsInHand(TestingGameState.MAX_CARDS_IN_HAND);
        }


        public override void OnLogic()
        {
            if (_timer.Elapsed < 1)
                return;
            
            Debug.Log("Cards drawn.");
            fsm.StateCanExit();
        }
    }

    
    public class AskMoveOrPlayCardsState : StateBase
    {
        public AskMoveOrPlayCardsState() : base(needsExitTime:false)
        {
        }


        public override void OnEnter()
        {
            TestingGameState.SetAllowEndTurn(true);
            
            Debug.Log("Move [M] or play cards [P]?");
            base.OnEnter();
        }
        
        
        public override void OnLogic()
        {
            base.OnLogic();
            
            if (Input.GetKeyDown(KeyCode.M))
                ((HybridStateMachine)fsm).Trigger("OnPlayerSelectMove");
            else if (Input.GetKeyDown(KeyCode.P))
                ((HybridStateMachine)fsm).Trigger("OnPlayerSelectPlayCards");
        }
    }

    
    public class MoveState : StateBase
    {
        public MoveState() : base(needsExitTime:true)
        {
        }


        public override void OnEnter()
        {
            Debug.Log("Move [X]");
            TestingGameState.SetRemainingMovements(TestingGameState.ALLOWED_MOVEMENTS);
            base.OnEnter();
        }
        
        
        public override void OnLogic()
        {
            base.OnLogic();
            
            // Moving
            if (TestingGameState.HasMovements && Input.GetKeyDown(KeyCode.X))
            {
                TestingGameState.Move();

                if (!TestingGameState.HasMovements)
                    fsm.StateCanExit();
            }
        }
    }

    
    public class DiscardCardsState : StateBase
    {
        public DiscardCardsState() : base(needsExitTime:true)
        {
        }


        public override void OnEnter()
        {
            Debug.Log("Discard a card [D] or skip [S]");
            
            // Pre-emptively exit if there are no cards to discard
            if (!TestingGameState.HasHandCards)
                fsm.StateCanExit();
            
            base.OnEnter();
        }
        
        
        public override void OnLogic()
        {
            base.OnLogic();
            
            // Skipping
            if (Input.GetKeyDown(KeyCode.S))
            {
                fsm.StateCanExit();
                return;
            }
            
            // Discarding
            if (TestingGameState.HasHandCards && Input.GetKeyDown(KeyCode.D))
            {
                TestingGameState.DiscardCard();

                // Exit if there are no more cards to discard
                if (!TestingGameState.HasHandCards)
                    fsm.StateCanExit();
            }
        }


        public override void OnExitRequest()
        {
            
        }
    }

    
    public class ActivateCardsState : StateBase
    {
        public ActivateCardsState() : base(needsExitTime:true)
        {
        }


        public override void OnEnter()
        {
            Debug.Log("Activate a card [A] or skip [S]");
            base.OnEnter();
        }
        
        
        public override void OnLogic()
        {
            base.OnLogic();
            
            // Skipping
            if (Input.GetKeyDown(KeyCode.S))
            {
                fsm.StateCanExit();
                return;
            }
            
            // Activating
            if (TestingGameState.HasHandCards && Input.GetKeyDown(KeyCode.A))
            {
                TestingGameState.ActivateCard();

                // Exit if there are no more cards to activate
                if (!TestingGameState.HasHandCards)
                    fsm.StateCanExit();
            }
        }


        public override void OnExitRequest()
        {
            
        }
    }

    
    public class PlayCardsState : StateBase
    {
        public PlayCardsState() : base(needsExitTime:true)
        {
        }


        public override void OnEnter()
        {
            Debug.Log("Play a card [P]");
            base.OnEnter();
        }
        
        
        public override void OnLogic()
        {
            base.OnLogic();
            
            // Activating
            if (TestingGameState.HasActivatedCards && Input.GetKeyDown(KeyCode.P))
            {
                TestingGameState.PlayCard();

                // Exit if there are no more cards to play
                if (!TestingGameState.HasActivatedCards)
                    fsm.StateCanExit();
            }
        }
    }
}