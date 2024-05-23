using Singletons;
using UnityEngine;

namespace Cameras
{
    public class CameraController : SingletonBehaviour<CameraController>
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private float _dragSpeed = 18f;

        [SerializeField]
        private float _maxDistanceFromOrigin = 5f;

        [SerializeField]
        private float _zOffset = -10f;

        private Vector3 _dragOrigin;
        private Vector3 _origin;
        
        public Camera Camera => _camera;


        public void SetOrigin(Vector2 origin)
        {
            _origin = new Vector3(origin.x, origin.y, _zOffset);
            transform.position = _origin;
        }


        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(2))
                _dragOrigin = _camera.ScreenToViewportPoint(Input.mousePosition);

            if (!Input.GetMouseButton(2))
                return;
            
            Vector3 difference = _dragOrigin - _camera.ScreenToViewportPoint(Input.mousePosition);

            Vector3 newPosition = _camera.transform.position + difference * _dragSpeed;
            newPosition.x = Mathf.Clamp(newPosition.x, _origin.x -_maxDistanceFromOrigin, _origin.x + _maxDistanceFromOrigin);
            newPosition.y = Mathf.Clamp(newPosition.y, _origin.y -_maxDistanceFromOrigin, _origin.y + _maxDistanceFromOrigin);

            _camera.transform.position = newPosition;

            _dragOrigin = _camera.ScreenToViewportPoint(Input.mousePosition);
        }
    }
}