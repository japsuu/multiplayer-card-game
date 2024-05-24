using System;
using UnityEngine;

namespace Utils
{
    public class WaitForEvent : CustomYieldInstruction, IDisposable
    {
        private bool _isEventTriggered;
        private Action _event;


        public override bool keepWaiting => !_isEventTriggered;


        public WaitForEvent(Action eventTrigger)
        {
            _event = eventTrigger;
            _event += TriggerEvent;
        }


        public void Dispose()
        {
            _event -= TriggerEvent;
        }


        private void TriggerEvent()
        {
            _isEventTriggered = true;
        }
    }
}