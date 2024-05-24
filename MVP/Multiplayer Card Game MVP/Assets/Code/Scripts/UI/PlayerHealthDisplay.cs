using Entities.Players;

namespace UI
{
    public class PlayerHealthDisplay : HealthDisplay
    {
        private void OnEnable()
        {
            if (PlayerCharacter.LocalPlayer != null)
                SetTargetEntity(PlayerCharacter.LocalPlayer);
            
            PlayerCharacter.LocalPlayerCreated += SetTargetEntity;
        }


        private void OnDisable()
        {
            PlayerCharacter.LocalPlayerCreated -= SetTargetEntity;
        }
    }
}