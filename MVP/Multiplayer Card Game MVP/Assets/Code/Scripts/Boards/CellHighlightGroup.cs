using System.Collections.Generic;
using UnityEngine;

namespace Boards
{
    /// <summary>
    /// A group on multiple cell highlighters.
    /// Used to highlight multiple cells at once, for example to show the area of effect of an attack.
    /// </summary>
    public class CellHighlightGroup : MonoBehaviour
    {
        [SerializeField]
        private CellHighlighter _highlighterPrefab;
        
        private readonly List<CellHighlighter> _highlighters = new();
        private CellHighlighter.PulseSettings _pulseSettings;
        
        
        /// <summary>
        /// Sets the pulse settings for every highlighter in this group.
        /// </summary>
        /// <param name="pulseSettings"></param>
        public void SetPulseSettings(CellHighlighter.PulseSettings pulseSettings)
        {
            _pulseSettings = pulseSettings;
            foreach (CellHighlighter highlighter in _highlighters)
                highlighter.Initialize(_pulseSettings);
        }
        
        
        /// <summary>
        /// Adds a new highlighter to this group, relative to the group's position.
        /// </summary>
        /// <param name="relativePosition">The cell's position relative to the group's position.</param>
        /// <param name="color">The highlighter's color.</param>
        public void AddRelativeHighlighter(Vector2Int relativePosition, Color color)
        {
            CellHighlighter highlighter = Instantiate(_highlighterPrefab, transform);
            highlighter.transform.position = (Vector3Int)relativePosition;
            highlighter.Initialize(color, _pulseSettings);
            _highlighters.Add(highlighter);
        }
        
        
        /// <summary>
        /// Removes/deletes all highlighters from this group.
        /// </summary>
        public void ResetHighlighters()
        {
            foreach (CellHighlighter highlighter in _highlighters)
                Destroy(highlighter.gameObject);
            _highlighters.Clear();
        }
    }
}