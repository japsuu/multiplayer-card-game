using System.Collections;
using Boards;
using Cards.AttackPatterns;
using Entities.Players;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cards.Data
{
    [CreateAssetMenu(menuName = "Cards/Create Attack Card", fileName = "Card_Attack_", order = 0)]
    public class AttackCard : CardData
    {
        [SerializeField]
        private int _damage = 10;

        [FormerlySerializedAs("_attackPattern")]
        [SerializeField]
        private CellPattern _cellPattern;

        public override CellPattern CellPattern => _cellPattern;


        public override IEnumerator OnPlayed(CardInstance card, Vector2Int cell)
        {
            yield return base.OnPlayed(card, cell);
            
            foreach (Vector2Int damagedPos in CellPattern.GetCells(cell))
            {
                if (BoardManager.Instance.TryGetCell(damagedPos, out BoardCell damagedCell))
                {
                    damagedCell.Occupant?.Damageable?.TakeDamage(_damage, PlayerCharacter.LocalPlayer);
                }
            }
        }
    }
}