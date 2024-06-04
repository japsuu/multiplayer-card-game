using System;
using PhaseSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class DebugWindowManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _contentsRoot;
        
        
        [Header("Phase")]
        
        [FormerlySerializedAs("_text")]
        [SerializeField]
        private TMP_Text _phaseText;

        [SerializeField]
        private string _phaseFormat = "Phase: {0}";
        
        private bool _enabled = true;


        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.F1))
                return;
            
            _enabled = !_enabled;
            _contentsRoot.SetActive(_enabled);
        }


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
            _phaseText.text = string.Format(_phaseFormat, obj.Name);
        }
    }
}