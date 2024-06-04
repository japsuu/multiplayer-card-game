using System;
using PhaseSystem;
using UnityEngine.Events;

namespace UI
{
    public class EndTurnButton : ToggleableEventButton
    {
        protected override UnityAction ClickAction => GameLoopManager.RequestEndTurn;


        protected override void SubscribeToEvents(Action<bool> setVisibility)
        {
            GameLoopManager.RequestShowEndTurnButton += setVisibility;
        }


        protected override void UnsubscribeFromEvents(Action<bool> setVisibility)
        {
            GameLoopManager.RequestShowEndTurnButton -= setVisibility;
        }
    }
}