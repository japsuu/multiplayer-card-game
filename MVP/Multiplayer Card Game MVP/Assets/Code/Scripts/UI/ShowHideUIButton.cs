using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShowHideUIButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        
        public GameObject Target;
        
        private bool _isVisible = true;
        
        
        private void Awake()
        {
            _button.onClick.AddListener(OnPressed);
        }


        private void OnEnable()
        {
            _isVisible = true;
            Target.SetActive(true);
        }


        private void OnDisable()
        {
            _isVisible = false;
            Target.SetActive(false);
        }


        private void OnPressed()
        {
            Target.SetActive(!_isVisible);
            _isVisible = !_isVisible;
        }
    }
}