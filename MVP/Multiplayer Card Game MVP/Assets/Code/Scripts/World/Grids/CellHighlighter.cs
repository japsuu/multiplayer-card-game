using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace World.Grids
{
    public class CellHighlighter : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        [Header("Pulsing")]

        [SerializeField]
        private bool _enablePulsing = true;

        [SerializeField]
        private float _pulseSpeed = 1f;

        [SerializeField]
        private float _pulseMinAlpha = 0.2f;
        
        [SerializeField]
        private float _pulseMaxAlpha = 0.8f;

        private TweenerCore<Color, Color, ColorOptions> _tweener;


        private void Awake()
        {
            if (!_enablePulsing)
                return;
            
            Color color = _spriteRenderer.color;
            color = new Color(color.r, color.g, color.b, _pulseMaxAlpha);
            _spriteRenderer.color = color;
        }


        private void Start()
        {
            if (!_enablePulsing)
                return;

            _tweener = _spriteRenderer.DOFade(_pulseMinAlpha, _pulseSpeed).SetLoops(-1, LoopType.Yoyo);
        }


        private void OnDestroy()
        {
            if (!_enablePulsing)
                return;

            _tweener.Kill();
        }


        public void SetHighlightColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}