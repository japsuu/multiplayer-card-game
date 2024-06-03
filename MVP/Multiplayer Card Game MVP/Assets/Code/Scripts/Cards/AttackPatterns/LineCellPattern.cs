using System.Collections.Generic;
using UnityEngine;

namespace Cards.AttackPatterns
{
    [CreateAssetMenu(menuName = "Patterns/Create LineGridPattern", fileName = "LineGridPattern_", order = 0)]
    public class LineCellPattern : CellPattern
    {
        private enum LineDirection
        {
            Horizontal,
            Vertical
        }
        [SerializeField]
        private LineDirection _direction;
        
        [SerializeField]
        private int _range = 2;
        
        
        public override IEnumerable<Vector2Int> GetCells(Vector2Int origin)
        {
            if (_direction == LineDirection.Horizontal)
            {
                for (int x = -_range; x <= _range; x++)
                {
                    yield return origin + new Vector2Int(x, 0);
                }
            }
            else
            {
                for (int y = -_range; y <= _range; y++)
                {
                    yield return origin + new Vector2Int(0, y);
                }
            }
        }
    }
}