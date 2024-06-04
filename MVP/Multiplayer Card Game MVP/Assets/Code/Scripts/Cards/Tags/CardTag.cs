using System.Collections;
using Entities;
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
        
        
        public virtual IEnumerator OnAttacked(BoardEntity damagingEntity, int damageAmount)
        {
            yield break;
        }
    }
}