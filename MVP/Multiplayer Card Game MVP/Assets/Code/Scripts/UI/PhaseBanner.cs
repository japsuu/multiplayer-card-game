using System.Collections;
using DG.Tweening;
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

        [SerializeField]
        private float _tweenTargetScale = 4f;
        
        [SerializeField]
        private float _tweenGrowLength = 0.5f;
        
        [SerializeField]
        private float _tweenStayLength = 1f;
        
        [SerializeField]
        private float _tweenShrinkLength = 1f;
        
        
        public IEnumerator DisplayPhase(string phaseName)
        {
            _phaseText.text = $"Current phase: {phaseName}";
            
            // Tween the scale of the text
            yield return _phaseText.transform.DOScale(_tweenTargetScale, _tweenGrowLength).WaitForCompletion();
            yield return new WaitForSeconds(_tweenStayLength);
            yield return _phaseText.transform.DOScale(1f, _tweenShrinkLength).WaitForCompletion();
        }
    }
}