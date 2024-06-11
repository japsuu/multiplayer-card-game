using System;
using StateManagement;
using UnityEngine;

namespace UI.ChoicePanel
{
    public class ActionChoicePanel : MonoBehaviour
    {
        [SerializeField]
        private ActionChoiceButton _buttonPrefab;
        
        [SerializeField]
        private Transform _buttonContainer;
        
        [SerializeField]
        private GameObject _content;


        private void Awake()
        {
            _content.SetActive(false);
        }


        private void OnEnable()
        {
            GameState.OnRequestChoice += OnRequestChoice;
        }
        
        
        private void OnDisable()
        {
            GameState.OnRequestChoice -= OnRequestChoice;
        }


        private void OnRequestChoice(string[] choices, Action<string> callback)
        {
            _content.SetActive(true);
            
            foreach (Transform child in _buttonContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (string choice in choices)
            {
                ActionChoiceButton button = Instantiate(_buttonPrefab, _buttonContainer);
                button.Initialize(choice, c => OnChoiceSelected(c, callback));
            }
        }
        
        
        private void OnChoiceSelected(string choice, Action<string> callback)
        {
            callback(choice);
            _content.SetActive(false);
        }
    }
}