using Singletons;
using UnityEngine;

namespace UI
{
    public class CanvasManager : SingletonBehaviour<CanvasManager>
    {
        [SerializeField]
        private Canvas _overlayCanvas;

        [SerializeField]
        private RectTransform _overlayCanvasRoot;

        public Canvas OverlayCanvas => _overlayCanvas;
        public RectTransform OverlayCanvasRoot => _overlayCanvasRoot;
    }
}