using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Utils.Singletons;

namespace UI
{
    /// <summary>
    /// Displays the current phase of the game.
    /// </summary>
    public class PhaseBanner : HiddenSingletonBehaviour<PhaseBanner>
    {
        [Serializable]
        private class TweenSettings
        {
            public float TweenTargetScale = 4f;
            public float TweenGrowLength = 0.5f;
            public float TweenStayLength = 1f;
            public float TweenShrinkLength = 1f;
        }
        
        [SerializeField]
        private TMP_Text _phaseText;
        
        [SerializeField]
        private TweenSettings _tweenSettingsMinor;
        
        [SerializeField]
        private TweenSettings _tweenSettingsMajor;
        
        
        public static IEnumerator DisplayPhase(string phaseName, bool major) => Instance.DisplayPhaseInternal(phaseName, major);
        
        private IEnumerator DisplayPhaseInternal(string phaseName, bool major)
        {
            _phaseText.text = $"Current phase: {phaseName}";
            
            TweenSettings settings = major ? _tweenSettingsMajor : _tweenSettingsMinor;
            
            // Tween the scale of the text
            yield return _phaseText.transform.DOScale(settings.TweenTargetScale, settings.TweenGrowLength).WaitForCompletion();
            yield return new WaitForSeconds(settings.TweenStayLength);
            yield return _phaseText.transform.DOScale(1f, settings.TweenShrinkLength).WaitForCompletion();
        }
    }
}