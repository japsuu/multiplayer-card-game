using System.Collections;
using Entities.Enemies;
using UnityEngine;

namespace Cards.Tags
{
    public abstract class CardTag : ScriptableObject
    {
        public abstract string Name { get; }
        
        /// <summary>
        /// Describes when the trigger is executed.
        /// Used in the card description as "[TriggerDescription], [ActionDescription]".
        /// </summary>
        protected abstract string TriggerDescription { get; }

        /// <summary>
        /// Describes what the action does.
        /// Used in the card description as "[TriggerDescription], [ActionDescription]".
        /// </summary>
        protected abstract string ActionDescription { get; }
        
        public string Description => $"{TriggerDescription}, {ActionDescription}";

        
        public virtual IEnumerator OnTurnStart()
        {
            yield break;
        }
        
        
        public virtual IEnumerator OnTurnEnd()
        {
            yield break;
        }
        
        
        public virtual IEnumerator OnDrawn()
        {
            yield break;
        }
        
        
        public virtual IEnumerator OnDiscarded()
        {
            yield break;
        }
        
        
        public virtual IEnumerator OnActivated()
        {
            yield break;
        }
        
        
        public virtual IEnumerator OnDeactivated()
        {
            yield break;
        }
        

        public virtual IEnumerator OnPlayed()
        {
            yield break;
        }
        
        
        public virtual IEnumerator OnEnemyAttacked(EnemyCharacter enemy)
        {
            yield break;
        }
    }
    
    public class NimbleTag : CardTag
    {
        [SerializeField]
        [Range(1, 3)]
        private int _movementRangeIncrease = 1;
        
        public override string Name => "Nimble";
        protected override string TriggerDescription => "While activated";
        protected override string ActionDescription => "Increase movement range by {_movementRangeIncrease}";
        
        
        public override IEnumerator OnPlayed()
        {
            // Draw a card
            yield break;
        }
    }
}