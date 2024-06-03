using System.Collections.Generic;
using UnityEngine;

namespace Cards.AttackPatterns
{
    [CreateAssetMenu(menuName = "Patterns/Create SquareGridPattern", fileName = "SquareGridPattern_", order = 0)]
    public class SquareCellPattern : CellPattern
    {
        [SerializeField]
        private int _range = 1;
        
        
        public override IEnumerable<Vector2Int> GetCells(Vector2Int origin)
        {
            for (int x = -_range; x <= _range; x++)
            {
                for (int y = -_range; y <= _range; y++)
                {
                    yield return origin + new Vector2Int(x, y);
                }
            }
        }
    }
}