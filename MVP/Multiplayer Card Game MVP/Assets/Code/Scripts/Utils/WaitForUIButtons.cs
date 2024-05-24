using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Utils
{
    public class WaitForUIButtons : CustomYieldInstruction, IDisposable
    {
        private struct ButtonCallback
        {
            public Button Button;
            public UnityAction Listener;
        }

        private readonly List<ButtonCallback> _mButtons = new();
        private System.Action<Button> _mCallback;

        public override bool keepWaiting => PressedButton == null;
        public Button PressedButton { get; private set; }


        public WaitForUIButtons(System.Action<Button> aCallback, params Button[] aButtons)
        {
            _mCallback = aCallback;
            _mButtons.Capacity = aButtons.Length;
            foreach (Button b in aButtons)
            {
                if (b == null)
                    continue;
                ButtonCallback bc = new()
                {
                    Button = b
                };
                bc.Listener = () => OnButtonPressed(bc.Button);
                _mButtons.Add(bc);
            }

            Reset();
        }


        public WaitForUIButtons(params Button[] aButtons) : this(null, aButtons)
        {
        }


        private void OnButtonPressed(Button button)
        {
            PressedButton = button;
            RemoveListeners();
            if (_mCallback != null)
                _mCallback(button);
        }


        private void InstallListeners()
        {
            foreach (ButtonCallback bc in _mButtons)
                if (bc.Button != null)
                    bc.Button.onClick.AddListener(bc.Listener);
        }


        private void RemoveListeners()
        {
            foreach (ButtonCallback bc in _mButtons)
                if (bc.Button != null)
                    bc.Button.onClick.RemoveListener(bc.Listener);
        }


        public new WaitForUIButtons Reset()
        {
            RemoveListeners();
            PressedButton = null;
            InstallListeners();
            base.Reset();
            return this;
        }


        public WaitForUIButtons ReplaceCallback(System.Action<Button> aCallback)
        {
            _mCallback = aCallback;
            return this;
        }


        public void Dispose()
        {
            RemoveListeners();
            _mCallback = null;
            _mButtons.Clear();
        }
    }
}