using System.Collections;
using Boards;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cards.Data
{
    /// <summary>
    /// Contains the immutable data of a card.
    /// </summary>
    public abstract class CardData : ScriptableObject
    {
        [SerializeField]
        private string _cardName;

        [SerializeField]
        private string _description;

        [SerializeField]
        private Sprite _sprite;

        [FormerlySerializedAs("_playType")]
        [SerializeField]
        private CardPlayTarget _playTarget;

        [SerializeField]
        private int _manaCost;
        public string CardName => _cardName;
        public string Description => _description;
        public Sprite Sprite => _sprite;
        public int ManaCost => _manaCost;
        public CardPlayTarget PlayTarget => _playTarget;
        
        
        /// <summary>
        /// Called when the user has started to drag the card.
        /// Used to initialize the highlighter group to e.g. show the damage area of the card.
        /// </summary>
        /// <param name="highlighterGroup"></param>
        public virtual void BuildHighlighter(CellHighlightGroup highlighterGroup) { }
        
        /// <summary>
        /// Called when the user is dragging the card.
        /// Used to update the data of the highlighter group.
        /// </summary>
        /// <param name="highlighterGroup">The highlighter group to update.</param>
        /// <param name="currentCell">The current cell the user is dragging the card over.</param>
        public virtual void UpdateHighlighter(CellHighlightGroup highlighterGroup, Vector2Int currentCell) { }
        
        /// <summary>
        /// Called when the user has played the card.
        /// </summary>
        /// <param name="cell">The cell the user has dropped the card on.</param>
        /// <returns></returns>
        public abstract IEnumerator OnPlay(Vector2Int cell);
    }
}