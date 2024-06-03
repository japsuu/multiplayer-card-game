namespace Cards
{
    public interface ICardInstanceReceiver
    {
        public bool CanReceiveCard(CardInstance card);
        public void ReceiveCard(CardInstance card);
        public void OnHoverEnter(CardInstance card);
        public void OnHover(CardInstance cardInstance);
        public void OnHoverExit(CardInstance cardInstance);
    }
}