using Boards;
using DamageSystem;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EntityHealth))]
    public class EnemyCharacter : MonoBehaviour, ICellOccupant
    {
        private EntityHealth _health;
        
        public IDamageable Damageable => _health;


        private void Awake()
        {
            _health = GetComponent<EntityHealth>();
        }
    }
}