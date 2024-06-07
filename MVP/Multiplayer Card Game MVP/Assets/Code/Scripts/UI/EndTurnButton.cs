using System;
using StateManagement;
using UnityEngine.Events;

namespace UI
{
    public class EndTurnButton : ToggleableEventButton
    {
        protected override UnityAction ClickAction => GameManager.EndTurn;


        protected override void SubscribeToEvents(Action<bool> setVisibility)
        {
            GameManager.RequestShowEndTurnButton += setVisibility;
        }


        protected override void UnsubscribeFromEvents(Action<bool> setVisibility)
        {
            GameManager.RequestShowEndTurnButton -= setVisibility;
        }
    }
}