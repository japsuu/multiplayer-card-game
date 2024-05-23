using DamageSystem;
using JetBrains.Annotations;

namespace Boards
{
    public interface ICellOccupant
    {
        [CanBeNull]
        public IDamageable Damageable { get; }
    }
}