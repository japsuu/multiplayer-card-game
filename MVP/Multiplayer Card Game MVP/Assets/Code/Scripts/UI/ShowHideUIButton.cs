using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShowHideUIButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        
        public GameObject Target;
        
        private bool _isVisible;
        
        
        private void Awake()
        {
            _button.onClick.AddListener(OnPressed);
        }


        private void OnPressed()
        {
            Target.SetActive(!_isVisible);
        }
    }
}