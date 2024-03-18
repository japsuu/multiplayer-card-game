using System.Collections.Generic;
using UnityEngine;

namespace Cards.AttackPatterns
{
    [CreateAssetMenu(menuName = "Patterns/Create CrossGridPattern", fileName = "CrossGridPattern_", order = 0)]
    public class CrossGridPattern : GridPattern
    {
        [SerializeField]
        private int _horizontalRange = 2;
        
        [SerializeField]
        private int _verticalRange = 2;
        
        
        public override IEnumerable<Vector2Int> GetCells(Vector2Int origin)
        {
            yield return origin;
            for (int x = -_horizontalRange; x <= _horizontalRange; x++)
            {
                if (x == 0)
                    continue;
                yield return origin + new Vector2Int(x, 0);
            }
            for (int y = -_verticalRange; y <= _verticalRange; y++)
            {
                if (y == 0)
                    continue;
                yield return origin + new Vector2Int(0, y);
            }
        }
    }
}