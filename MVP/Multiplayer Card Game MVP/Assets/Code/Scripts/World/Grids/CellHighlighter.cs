using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using JetBrains.Annotations;
using UnityEngine;

namespace World.Grids
{
    public class CellHighlighter : MonoBehaviour
    {
        [Serializable]
        public class PulseSettings
        {
            public float Speed;
            public float MinAlpha;
            public float MaxAlpha;
            
            public bool EnablePulsing => Speed > 0;
            
            
            public PulseSettings(float speed = 1.5f, float minAlpha = 0.4f, float maxAlpha = 0.9f)
            {
                Speed = speed;
                MinAlpha = minAlpha;
                MaxAlpha = maxAlpha;
            }
            
            
            public static PulseSettings NoPulse => new PulseSettings(0);
            public static PulseSettings Attack => new PulseSettings(1.5f, 0.7f, 0.9f);
        }
        
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        [Header("Pulsing")]

        [SerializeField]
        [CanBeNull]
        private PulseSettings _pulseSettings;
        
        private TweenerCore<Color, Color, ColorOptions> _tweener;
        
        
        public void Initialize(Color color, [CanBeNull] PulseSettings pulseSettings)
        {
            _spriteRenderer.color = color;
            _pulseSettings = pulseSettings;
        }
        
        
        public void Initialize([CanBeNull] PulseSettings pulseSettings)
        {
            _pulseSettings = pulseSettings;
        }


        private void Awake()
        {
            if (_pulseSettings == null || !_pulseSettings.EnablePulsing)
                return;
            
            Color color = _spriteRenderer.color;
            color = new Color(color.r, color.g, color.b, _pulseSettings.MaxAlpha);
            _spriteRenderer.color = color;
        }


        private void Start()
        {
            if (_pulseSettings == null || !_pulseSettings.EnablePulsing)
                return;

            _tweener = _spriteRenderer.DOFade(_pulseSettings.MinAlpha, _pulseSettings.Speed).SetLoops(-1, LoopType.Yoyo);
        }


        private void OnDestroy()
        {
            if (_pulseSettings == null || !_pulseSettings.EnablePulsing)
                return;

            _tweener.Kill();
        }
    }
}