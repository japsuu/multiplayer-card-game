using Boards;
using UnityEngine;
using Utils.Singletons;

namespace Cards
{
    /// <summary>
    /// Hearthstone-style targeting arrow.
    /// </summary>
    public class TargetingArrow : SingletonBehaviour<TargetingArrow>
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _lineZOffset;
        private bool _isActive;
        private Transform _origin;
        private Camera _camera;


        private void Awake()
        {
            _camera = Camera.main;
        }


        private void Update()
        {
            if (!_isActive)
                return;

            if (_origin == null)
            {
                Deactivate();
                return;
            }

            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 startPos = _origin.position;

            Vector3 endPos = BoardManager.Instance.TrySnapToCell(mouseWorldPos, out Vector3 cellPos) ? cellPos : mouseWorldPos;
            endPos.z = _lineZOffset;
            
            _lineRenderer.SetPosition(0, startPos);
            _lineRenderer.SetPosition(1, endPos);
        }
        
        
        public bool TryGetCell(out Vector2Int cell)
        {
            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            return BoardManager.Instance.TryGetWorldToCell(mouseWorldPos, out cell);
        }

        /// <summary>
        /// Activates the targeting arrow.
        /// </summary>
        /// <param name="origin">The transform the targeting arrow originates from.</param>
        public void Activate(Transform origin)
        {
            if (_isActive)
                return;
            
            _origin = origin;
            _lineRenderer.enabled = true;
            _isActive = true;
        }

        /// <summary>
        /// Deactivates the targeting arrow.
        /// </summary>
        public void Deactivate()
        {
            if (!_isActive)
                return;
            
            _lineRenderer.enabled = false;
            _isActive = false;
        }
    }
}