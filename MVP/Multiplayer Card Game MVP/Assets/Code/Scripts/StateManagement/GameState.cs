using System;
using Cards;
using Entities.Players;

namespace StateManagement
{
    public enum PlayerAction
    {
        None,
        Move,
        PlayCards
    }
    
    /// <summary>
    /// Stores the current game state.
    /// </summary>
    public static class GameState
    {
        public static event Action<string[], Action<string>> OnRequestChoice;
        
        public static PlayerAction SelectedPlayerAction { get; private set; }
        
        public static bool ShowHand { get; private set; }
        public static bool AllowMovement { get; private set; }
        public static bool AllowCardDiscard { get; private set; }
        public static bool AllowCardActivation { get; private set; }
        public static bool AllowCardPlay { get; private set; }
        public static bool AllowSkip { get; private set; }
        public static bool AllowEndTurn { get; private set; }

        public static bool HasHandCards => PlayerHandManager.Instance.CardCount > 0;
        public static bool HasActivatedCards => CardActivationSlot.ActivatedCardCount > 0;
        public static bool HasLocalPlayerMoved => PlayerCharacter.LocalPlayer.Movement.HasPlayerMoved;
        
        
        public static void RequestChoice(string[] choices, Action<string> callback)
        {
            OnRequestChoice?.Invoke(choices, callback);
        }
        
        
        public static void SetSelectedPlayerAction(PlayerAction action)
        {
            SelectedPlayerAction = action;
        }


        public static void SetShowHand(bool value)
        {
            ShowHand = value;
            if (value)
                PlayerHandManager.Instance.ShowHand();
            else
                PlayerHandManager.Instance.HideHand();
        }
        
        
        public static void SetAllowMovement(bool value)
        {
            AllowMovement = value;
            if (value)
                PlayerCharacter.LocalPlayer.Movement.EnableMovement();
            else
                PlayerCharacter.LocalPlayer.Movement.DisableMovement();
        }
        
        
        public static void SetAllowCardDiscard(bool value)
        {
            AllowCardDiscard = value;
            PlayerHandManager.Instance.AllowDiscardCards(value);
        }
        
        
        public static void SetAllowCardActivation(bool value)
        {
            AllowCardActivation = value;
        }
        
        
        public static void SetAllowCardPlay(bool value)
        {
            AllowCardPlay = value;
        }
        
        
        public static void SetAllowSkip(bool value)
        {
            AllowSkip = value;
            GameManager.ShowSkipButton(value);
        }
        
        
        public static void SetAllowEndTurn(bool value)
        {
            AllowEndTurn = value;
            GameManager.ShowEndTurnButton(value);
        }
    }
}