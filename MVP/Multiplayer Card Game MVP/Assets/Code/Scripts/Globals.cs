/// <summary>
/// Provides a place to store global variables that can be accessed from any script.
/// </summary>
public static class Globals
{
    public static bool VerboseEnemyMovement = false;
    
    /// <summary>
    /// True to move played cards directly to the discard pile, false to keep them in the card activation slot they were played from.
    /// </summary>
    public static bool DiscardPlayedCards = false;
}