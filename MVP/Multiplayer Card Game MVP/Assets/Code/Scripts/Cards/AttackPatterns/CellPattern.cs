using System.Collections.Generic;
using UnityEngine;

namespace Cards.AttackPatterns
{
    public abstract class CellPattern : ScriptableObject
    {
        public abstract IEnumerable<Vector2Int> GetCells(Vector2Int origin);
    }
}