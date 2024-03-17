using UnityEngine;

namespace World.Grids
{
    public class CellHighlighter : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        
        public void SetHighlightColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}