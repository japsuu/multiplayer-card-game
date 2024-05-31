using System;
using PhaseSystem;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PhaseDebugText : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;

        [SerializeField]
        private string _format = "Phase: {0}";


        private void OnEnable()
        {
            GameLoopManager.PhaseChange += OnPhaseChange;
        }
        
        
        private void OnDisable()
        {
            GameLoopManager.PhaseChange -= OnPhaseChange;
        }


        private void OnPhaseChange(GamePhase obj)
        {
            _text.text = string.Format(_format, obj.Name);
        }
    }
}