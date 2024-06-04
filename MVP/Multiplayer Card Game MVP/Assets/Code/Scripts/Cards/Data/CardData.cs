using System.Collections;
using System.Collections.Generic;
using Cards.AttackPatterns;
using Cards.Tags;
using Entities;
using UnityEngine;

namespace Cards.Data
{
    /// <summary>
    /// Contains the immutable data of a card.
    /// </summary>
    public abstract class CardData : ScriptableObject
    {
        [Header("Card Data")]
        
        [SerializeField]
        private string _cardName;

        [SerializeField]
        [TextArea(3, 5)]
        private string _description;

        [SerializeField]
        private Sprite _sprite;

        [Header("Tags")]
        
        [SerializeField]
        private CardTag[] _tags;
        
        public abstract CellPattern CellPattern { get; }
        
        public string CardName => _cardName;
        public string Description => _description;
        public Sprite Sprite => _sprite;
        public IEnumerable<CardTag> Tags => _tags;
        
        /// <summary>
        /// Whether the card can be played or not (if it affects any cells when played).
        /// If the card doesn't affect any cells when played, this is false.
        /// As an example a card that only has passive effects while activated has this set to false.
        /// </summary>
        public bool CanBePlayed => CellPattern != null;


        protected virtual IEnumerator ApplyCellEffects(Vector2Int cell)
        {
            yield break;
        }
        
        
        public IEnumerator OnTurnStart(CardInstance card)
        {
            if (card.HasBeenPlayed)
                yield break;
            
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnTurnStart();
            }
        }
        
        
        public IEnumerator OnTurnEnd(CardInstance card)
        {
            if (card.HasBeenPlayed)
                yield break;

            foreach (CardTag tag in _tags)
            {
                yield return tag.OnTurnEnd();
            }
        }
        
        
        public IEnumerator OnDrawn(CardInstance card)
        {
            if (card.HasBeenPlayed)
                yield break;
            
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnDrawn();
            }
        }
        
        
        public IEnumerator OnDiscarded(CardInstance card)
        {
            if (card.HasBeenPlayed)
                yield break;
            
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnDiscarded();
            }
        }
        
        
        public IEnumerator OnActivated(CardInstance card)
        {
            if (card.HasBeenPlayed)
                yield break;
            
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnActivated();
            }
        }
        
        
        public IEnumerator OnDeactivated(CardInstance card)
        {
            if (card.HasBeenPlayed)
                yield break;
            
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnDeactivated();
            }
        }
        

        public IEnumerator OnPlayed(CardInstance card, Vector2Int cell)
        {
            if (card.HasBeenPlayed)
                yield break;
            
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnPlayed();
            }

            yield return ApplyCellEffects(cell);
        }
        
        
        public IEnumerator OnAttacked(CardInstance card, BoardEntity damagingEntity, int damageAmount)
        {
            if (card.HasBeenPlayed)
                yield break;
            
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnAttacked(damagingEntity, damageAmount);
            }
        }
    }
}