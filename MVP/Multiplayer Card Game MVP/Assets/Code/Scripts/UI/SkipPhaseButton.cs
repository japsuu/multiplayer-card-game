using System;
using StateManagement;
using UnityEngine.Events;

namespace UI
{
    public class SkipPhaseButton : ToggleableEventButton
    {
        protected override UnityAction ClickAction => GameManager.SkipCurrentState;


        protected override void SubscribeToEvents(Action<bool> setVisibility)
        {
            GameManager.RequestShowSkipButton += setVisibility;
        }


        protected override void UnsubscribeFromEvents(Action<bool> setVisibility)
        {
            GameManager.RequestShowSkipButton -= setVisibility;
        }
    }
}