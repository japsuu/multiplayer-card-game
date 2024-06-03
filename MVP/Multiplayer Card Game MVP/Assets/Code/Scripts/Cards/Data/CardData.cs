using System.Collections;
using Cards.AttackPatterns;
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
        

#warning Change "cell" to an IBoardRegion
        public abstract IEnumerator ApplyBoardEffects(Vector2Int cell);
    }
}