using DamageSystem;
using Entities.Players;
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
            PlayerCharacter.LocalPlayerCreated += SubscribeToHealthUpdates;
            PlayerCharacter.LocalPlayerDestroyed += OnLocalPlayerDestroyed;
            
            if (PlayerCharacter.LocalPlayer != null)
                SubscribeToHealthUpdates(PlayerCharacter.LocalPlayer);
        }


        private void OnLocalPlayerDestroyed()
        {
            PlayerCharacter.LocalPlayer.Health.HealthChanged -= OnHealthChanged;
        }


        private void SubscribeToHealthUpdates(PlayerCharacter player)
        {
            player.Health.HealthChanged += OnHealthChanged;
            
            _maxHealth = player.Health.MaxHealth;
            Refresh(player.Health.CurrentHealth);
        }


        private void OnDisable()
        {
            PlayerCharacter.LocalPlayerCreated -= SubscribeToHealthUpdates;
            PlayerCharacter.LocalPlayerDestroyed -= OnLocalPlayerDestroyed;
            
            if (PlayerCharacter.LocalPlayer != null)
                PlayerCharacter.LocalPlayer.Health.HealthChanged -= OnHealthChanged;
        }


        private void OnHealthChanged(HealthChangedArgs args)
        {
            Refresh(args.NewHealth);
        }


        private void Refresh(int newHealth)
        {
            _slider.maxValue = _maxHealth;
            _slider.value = newHealth;
            _text.text = $"{newHealth}/{_maxHealth} hp";
        }
    }
}