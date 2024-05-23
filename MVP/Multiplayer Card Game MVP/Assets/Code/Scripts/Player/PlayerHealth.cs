using System;
using DamageSystem;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        public event Action<HealthChangedArgs> HealthChanged;
        public event Action Died;
        
        [SerializeField]
        private int _maxHealth = 100;

        public int CurrentHealth { get; private set; }
        
        public int MaxHealth => _maxHealth;
        
        
        private void Start()
        {
            CurrentHealth = _maxHealth;
        }
        
        
        public void TakeDamage(int damage)
        {
            int previousHealth = CurrentHealth;
            CurrentHealth -= damage;
            
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
            CurrentHealth = 0;
            Debug.Log("Player died!");
            
            HealthChanged?.Invoke(new HealthChangedArgs(previousHealth, CurrentHealth));
            Died?.Invoke();
        }
    }
}