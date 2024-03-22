using System.Collections.Generic;

namespace Cards
{
    /// <summary>
    /// In-memory representation of the player's hand.
    /// </summary>
    public class PlayerHand
    {
        /// <summary>
        /// All currently instantiated cards in the player's hand.
        /// </summary>
        public readonly List<CardInstance> Cards = new();
    }
}