using UnityEngine;

namespace _Development
{
    public static class TestingGameState
    {
        public const int ALLOWED_MOVEMENTS = 3;
        public const int MAX_ACTIVATED_CARDS = 3;
        public const int MAX_CARDS_IN_HAND = 5;
        
        public static bool AllowEndTurn { get; private set; }

        public static int CardsInHand { get; private set; }
        public static int RemainingMovements { get; private set; }
        public static int ActivatedCards { get; private set; }
        
        public static bool HasMovements => RemainingMovements > 0;
        public static bool HasHandCards => CardsInHand > 0;
        public static bool HasActivatedCards => ActivatedCards > 0;
        
        
        public static void SetAllowEndTurn(bool value)
        {
            AllowEndTurn = value;
            Debug.Log($"End turn button is now {(value ? "visible" : "hidden")}. Simulate with [Space].");
        }
        
        
        public static void SetCardsInHand(int value)
        {
            CardsInHand = value;
        }
        
        
        public static void SetRemainingMovements(int value)
        {
            RemainingMovements = value;
        }
        
        
        public static void Move()
        {
            if (RemainingMovements <= 0)
                throw new System.Exception("No more movements allowed.");
            
            // Move player character
            RemainingMovements--;
            Debug.Log("Player moved");
        }
        
        
        public static void DiscardCard()
        {
            if (!HasHandCards)
                throw new System.Exception("No cards in hand.");
            
            CardsInHand--;
            Debug.Log("Card discarded");
        }


        public static void ActivateCard()
        {
            if (!HasHandCards)
                throw new System.Exception("No cards in hand.");
            
            CardsInHand--;
            ActivatedCards = Mathf.Min(ActivatedCards + 1, MAX_ACTIVATED_CARDS);
            Debug.Log("Card activated");
        }
        
        
        public static void PlayCard()
        {
            if (ActivatedCards <= 0)
                throw new System.Exception("No cards activated.");
            
            ActivatedCards--;
            Debug.Log("Card played");
        }
    }
}