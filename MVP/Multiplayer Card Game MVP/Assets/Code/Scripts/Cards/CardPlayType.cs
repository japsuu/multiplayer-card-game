namespace Cards
{
    /// <summary>
    /// Determines how a card can be played.
    /// Affects for example if the game will draw a "Hearthstone-style" targeting arrow.
    /// </summary>
    public enum CardPlayType
    {
        /// <summary>
        /// The card will be played to the middle of the board.
        /// </summary>
        Anywhere,

        /// <summary>
        /// The card will be played to a cell on the board.
        /// Draws a "Hearthstone-style" targeting arrow.
        /// Example: Attack that spans multiple cells.
        /// </summary>
        Cell
    }
}