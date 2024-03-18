using System.Collections.Generic;
using UnityEngine;

namespace World.Grids
{
    public class CellHighlightGroup : MonoBehaviour
    {
        [SerializeField]
        private CellHighlighter _highlighterPrefab;
        
        private readonly List<CellHighlighter> _highlighters = new();
        private CellHighlighter.PulseSettings _pulseSettings;
        
        
        public void SetPulseSettings(CellHighlighter.PulseSettings pulseSettings)
        {
            _pulseSettings = pulseSettings;
            foreach (CellHighlighter highlighter in _highlighters)
                highlighter.Initialize(_pulseSettings);
        }
        
        
        public void AddRelativeHighlighter(Vector2Int relativePosition, Color color)
        {
            CellHighlighter highlighter = Instantiate(_highlighterPrefab, transform);
            highlighter.transform.position = (Vector3Int)relativePosition;
            highlighter.Initialize(color, _pulseSettings);
            _highlighters.Add(highlighter);
        }
        
        
        public void ResetHighlighters()
        {
            foreach (CellHighlighter highlighter in _highlighters)
                Destroy(highlighter.gameObject);
            _highlighters.Clear();
        }
    }
}