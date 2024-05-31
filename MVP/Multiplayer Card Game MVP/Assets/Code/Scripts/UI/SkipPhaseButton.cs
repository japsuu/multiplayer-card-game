using System;
using PhaseSystem;
using UnityEngine.Events;

namespace UI
{
    public class SkipPhaseButton : ToggleableEventButton
    {
        protected override UnityAction ClickAction => GameLoopManager.RequestSkipPhase;


        protected override void SubscribeToEvents(Action<bool> setVisibility)
        {
            GameLoopManager.RequestShowSkipButton += setVisibility;
        }


        protected override void UnsubscribeFromEvents(Action<bool> setVisibility)
        {
            GameLoopManager.RequestShowSkipButton -= setVisibility;
        }
    }
}