using UnityEngine;

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

        // [SerializeField]
        // private int _manaCost;
        
        public string CardName => _cardName;
        public string Description => _description;
        public Sprite Sprite => _sprite;
        // public int ManaCost => _manaCost;
        
        
        /// <summary>
        /// Called when the user has started to drag the card.
        /// </summary>
        public virtual void OnStartDrag(CardInstance draggedCard) { }
        
        
        /// <summary>
        /// Called when the user is dragging the card.
        /// </summary>
        public virtual void OnDrag(CardInstance draggedCard) { }
        
        
        /// <summary>
        /// Called when the user has stopped dragging the card.
        /// </summary>
        public virtual void OnEndDrag(CardInstance draggedCard, bool shouldPlayCard) { }
    }
}