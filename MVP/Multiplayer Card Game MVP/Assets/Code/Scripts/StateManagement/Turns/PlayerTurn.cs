using StateManagement.Turns.Players;
using UnityHFSM;

namespace StateManagement.Turns
{
    public class PlayerTurn : HybridStateMachine
    {
        public PlayerTurn() : base(needsExitTime:true)  // Manually define when to leave this state, with an exit transition
        {
            // ----- States -----
            AddState("TurnStart", new PlayerTurnStartState());
            AddState("DrawCards", new DrawCardsState());
            AddState("AskMoveOrPlayCards", new AskMoveOrPlayCardsState());
            AddState("Move", new MoveState());
            AddState("DiscardCards", new DiscardCardsState());
            AddState("ActivateCards", new ActivateCardsState());
            AddState("PlayCards", new PlayCardsState());
            AddState("TurnEnd", new PlayerTurnEndState());
            
            // ----- Transitions -----
            AddTransition(new Transition("TurnStart", "DrawCards"));
            AddTransition(new Transition("DrawCards", "AskMoveOrPlayCards"));
            AddTransition(new Transition("AskMoveOrPlayCards", "Move", _ => GameState.SelectedPlayerAction == PlayerAction.Move));
            AddTransition(new Transition("AskMoveOrPlayCards", "DiscardCards", _ => GameState.SelectedPlayerAction == PlayerAction.PlayCards));
            AddTransition(new Transition("Move", "TurnEnd"));
            AddTransition(new Transition("DiscardCards", "ActivateCards", _ => GameState.HasHandCards));  // Only allow activating cards if there are cards in hand
            AddTransition(new Transition("DiscardCards", "PlayCards"));
            AddTransition(new Transition("ActivateCards", "PlayCards", _ => GameState.HasActivatedCards));  // Only allow playing cards if there are activated cards
            AddTransition(new Transition("PlayCards", "TurnEnd"));
            
            // Exit transitions
            AddExitTransition(new Transition("TurnEnd", ""));
        }
    }
}