using Entities;
using JetBrains.Annotations;

namespace DamageSystem
{
    public interface IDamageable
    {
        public void TakeDamage(int damage, [CanBeNull] BoardEntity damagingEntity);
        public void Kill();
    }
}