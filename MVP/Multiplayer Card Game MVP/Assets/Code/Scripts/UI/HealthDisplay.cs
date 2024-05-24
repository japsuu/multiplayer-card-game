using DamageSystem;
using Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class HealthDisplay : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private TMP_Text _text;
        
        private int _maxHealth;
        private BoardEntity _targetEntity;
        
        
        protected void SetTargetEntity(BoardEntity targetEntity)
        {
            if (_targetEntity != null)
            {
                _targetEntity.Health.HealthChanged -= OnTargetHealthChanged;
                _targetEntity.Died -= OnTargetDied;
            }
            
            _targetEntity = targetEntity;

            if (_targetEntity == null)
                return;
            
            _targetEntity.Health.HealthChanged += OnTargetHealthChanged;
            _targetEntity.Died += OnTargetDied;
                
            _maxHealth = _targetEntity.Health.MaxHealth;
            Refresh(_targetEntity.Health.CurrentHealth);
        }


        private void OnTargetHealthChanged(HealthChangedArgs args)
        {
            Refresh(args.NewHealth);
        }


        private void OnTargetDied()
        {
            SetTargetEntity(null);
        }


        private void Refresh(int newHealth)
        {
            _slider.maxValue = _maxHealth;
            _slider.value = newHealth;
            _text.text = $"{newHealth}/{_maxHealth} hp";
        }
    }
}