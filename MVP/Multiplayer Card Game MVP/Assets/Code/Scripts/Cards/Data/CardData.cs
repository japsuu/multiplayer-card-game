using System.Collections;
using Cards.AttackPatterns;
using Cards.Tags;
using Entities.Enemies;
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
        private CardTag[] _tags;
        
        public abstract CellPattern CellPattern { get; }
        
        public string CardName => _cardName;
        public string Description => _description;
        public Sprite Sprite => _sprite;
        
        /// <summary>
        /// Whether the card can be played or not (if it affects any cells when played).
        /// If the card doesn't affect any cells when played, this is false.
        /// As an example a card that only has passive effects while activated has this set to false.
        /// </summary>
        public bool CanBePlayed => CellPattern != null;
        
        
        public virtual IEnumerator OnTurnStart()
        {
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnTurnStart();
            }
        }
        
        
        public virtual IEnumerator OnTurnEnd()
        {
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnTurnEnd();
            }
        }
        
        
        public virtual IEnumerator OnDrawn()
        {
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnDrawn();
            }
        }
        
        
        public virtual IEnumerator OnDiscarded()
        {
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnDiscarded();
            }
        }
        
        
        public virtual IEnumerator OnActivated()
        {
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnActivated();
            }
        }
        
        
        public virtual IEnumerator OnDeactivated()
        {
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnDeactivated();
            }
        }
        

#warning Change "cell" to an IBoardRegion
        public virtual IEnumerator OnPlayed(Vector2Int cell)
        {
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnPlayed();
            }
        }
        
        
        public virtual IEnumerator OnEnemyAttacked(EnemyCharacter enemy)
        {
            foreach (CardTag tag in _tags)
            {
                yield return tag.OnEnemyAttacked(enemy);
            }
        }
    }
}