using UnityEngine;

namespace StateManagement.Turns.Players
{
    public class AskMoveOrPlayCardsState : PlayerState
    {
        protected override bool AllowEndTurn => true;
        protected override bool ShouldShowHand => true;


        protected override void OnEnterState()
        {
            GameState.SetSelectedPlayerAction(PlayerAction.None);
            GameState.RequestChoice(new[] { "Move", "Play Cards" }, OnChoiceSelected);
        }


        public override void OnLogic()
        {
            if (GameState.SelectedPlayerAction != PlayerAction.None)
                fsm.StateCanExit();
        }
        
        
        private static void OnChoiceSelected(string choice)
        {
            switch (choice)
            {
                case "Move":
                    GameState.SetSelectedPlayerAction(PlayerAction.Move);
                    break;
                case "Play Cards":
                    GameState.SetSelectedPlayerAction(PlayerAction.PlayCards);
                    break;
                default:
                    Debug.LogError($"Invalid choice: {choice}");
                    break;
            }
        }
    }
}