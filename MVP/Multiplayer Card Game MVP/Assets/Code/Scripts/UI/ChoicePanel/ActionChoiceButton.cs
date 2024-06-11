using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ChoicePanel
{
    public class ActionChoiceButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private TMP_Text _choiceText;

        
        public void Initialize(string choice, Action<string> onChoiceSelected)
        {
            _choiceText.text = choice;
            _button.onClick.AddListener(() => onChoiceSelected(choice));
        }
    }
}