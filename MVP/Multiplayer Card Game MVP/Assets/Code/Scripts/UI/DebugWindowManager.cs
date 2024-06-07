using StateManagement;
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
            OnPhaseChange(GameManager.CurrentStateName);
            
            if (!Input.GetKeyDown(KeyCode.F1))
                return;
            
            _enabled = !_enabled;
            _contentsRoot.SetActive(_enabled);
        }


        private void OnPhaseChange(string newState)
        {
            _phaseText.text = string.Format(_phaseFormat, newState);
        }
    }
}