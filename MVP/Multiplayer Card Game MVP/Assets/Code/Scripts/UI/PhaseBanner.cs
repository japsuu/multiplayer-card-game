using System.Collections;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Displays the current phase of the game.
    /// </summary>
    public class PhaseBanner : SingletonBehaviour<PhaseBanner>
    {
        [SerializeField]
        private TMP_Text _phaseText;
        
        
        public IEnumerator DisplayPhase(string phaseName)
        {
            _phaseText.text = $"Current phase: {phaseName}";
            
            yield return new WaitForSeconds(3f);
        }
    }
}