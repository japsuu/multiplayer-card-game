using System.Collections;
using Entities;
using Entities.Players;
using UnityEngine;

namespace Cards.Tags
{
    [CreateAssetMenu(menuName = "Cards/Tags/Create ThornsTag", fileName = "ThornsTag", order = 0)]
    public class ThornsTag : CardTag
    {
        [SerializeField]
        [Range(1, 5)]
        private int _tagLevel = 1;
        
        [SerializeField]
        [Range(1, 10)]
        private int _retaliatedDamage = 5;
        
        public override string Name => $"Thorns ({_tagLevel})";
        protected override string TriggerDescription => "When attacked";
        protected override string ActionDescription => $"Deal {_retaliatedDamage} damage to the attacker";

        
        public override IEnumerator OnAttacked(BoardEntity damagingEntity, int damageAmount)
        {
            if (damagingEntity == PlayerCharacter.LocalPlayer)
                yield break;
            
            damagingEntity.Health.TakeDamage(_retaliatedDamage, PlayerCharacter.LocalPlayer);
        }
    }
}