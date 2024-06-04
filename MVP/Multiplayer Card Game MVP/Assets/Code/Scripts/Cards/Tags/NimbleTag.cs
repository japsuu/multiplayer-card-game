using System.Collections;
using Entities.Players;
using UnityEngine;

namespace Cards.Tags
{
    [CreateAssetMenu(menuName = "Cards/Tags/Create NimbleTag", fileName = "NimbleTag", order = 0)]
    public class NimbleTag : CardTag
    {
        [SerializeField]
        [Range(1, 3)]
        private int _movementRangeIncrease = 1;
        
        public override string Name => $"Nimble ({_movementRangeIncrease})";
        protected override string TriggerDescription => "While activated";
        protected override string ActionDescription => "Increase movement range by {_movementRangeIncrease}";
        
        
        public override IEnumerator OnActivated()
        {
            PlayerCharacter.LocalPlayer.Movement.ChangeMovementRangeModifier(_movementRangeIncrease);
            yield break;
        }
        
        
        public override IEnumerator OnDeactivated()
        {
            PlayerCharacter.LocalPlayer.Movement.ChangeMovementRangeModifier(-_movementRangeIncrease);
            yield break;
        }
    }
}