using System.Collections.Generic;
using UnityEngine;

namespace Cards.AttackPatterns
{
    [CreateAssetMenu(menuName = "Patterns/Create CircleGridPattern", fileName = "CircleGridPattern_", order = 0)]
    public class CircleGridPattern : GridPattern
    {
        [SerializeField]
        private int _range = 3;
        
        
        public override IEnumerable<Vector2Int> GetCells(Vector2Int origin)
        {
            for (int x = -_range; x <= _range; x++)
            {
                for (int y = -_range; y <= _range; y++)
                {
                    if (Vector2Int.Distance(origin, origin + new Vector2Int(x, y)) <= _range)
                        yield return origin + new Vector2Int(x, y);
                }
            }
        }
    }
}