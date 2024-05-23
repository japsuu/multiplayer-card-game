using System;
using NaughtyAttributes;
using UnityEngine;

namespace DamageSystem
{
    public class EntityHealth : MonoBehaviour, IDamageable
    {
        public event Action<HealthChangedArgs> HealthChanged;
        public event Action Died;
        
        [SerializeField]
        private int _maxHealth = 100;

        [SerializeField]
        [ReadOnly]
        private int _currentHealth;

        [SerializeField]
        [ReadOnly]
        private bool _isDead;

        public int CurrentHealth => _currentHealth;
        
        public bool IsDead => _isDead;
        public int MaxHealth => _maxHealth;
        
        
        private void Awake()
        {
            _currentHealth = _maxHealth;
        }
        
        
        public void TakeDamage(int damage)
        {
            int previousHealth = CurrentHealth;
            _currentHealth -= damage;
            
            if (CurrentHealth <= 0)
                Die();
            else
                HealthChanged?.Invoke(new HealthChangedArgs(previousHealth, CurrentHealth));
        }
        
        
        public void Kill()
        {
            Die();
        }
        
        
        private void Die()
        {
            int previousHealth = CurrentHealth;
            _currentHealth = 0;
            _isDead = true;
            Debug.LogWarning($"Entity {gameObject.name} died!");
            
            HealthChanged?.Invoke(new HealthChangedArgs(previousHealth, CurrentHealth));
            Died?.Invoke();
        }
    }
}