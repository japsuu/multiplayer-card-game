using System;
using DamageSystem;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private TMP_Text _text;
        
        private int _maxHealth;


        private void OnEnable()
        {
            PlayerHealth health = PlayerCharacter.LocalPlayer.Health;
            health.HealthChanged += OnHealthChanged;
            
            _maxHealth = health.MaxHealth;
            Refresh(health.CurrentHealth);
        }
        
        
        private void OnDisable()
        {
            PlayerCharacter.LocalPlayer.Health.HealthChanged -= OnHealthChanged;
        }


        private void OnHealthChanged(HealthChangedArgs args)
        {
            Refresh(args.NewHealth);
        }


        private void Refresh(int newHealth)
        {
            _slider.value = newHealth;
            _slider.maxValue = _maxHealth;
            _text.text = $"{newHealth}/{_maxHealth} hp";
        }
    }
}