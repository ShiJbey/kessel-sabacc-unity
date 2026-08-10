namespace KesselSabacc.Model
{
	public class CardDrawnEventData
	{
		public readonly int playerIndex;
		public readonly CardStack.DeckKind deck;
		public readonly Card card;

		public CardDrawnEventData(int playerIndex, CardStack.DeckKind deck , Card card)
		{
			this.playerIndex = playerIndex;
			this.deck = deck;
			this.card = card;
		}
	}

	public class CardDiscardedEventData
	{
		public readonly int playerIndex;
		public readonly int cardIndex;
		public readonly Card card;

		public CardDiscardedEventData(int playerIndex, int cardIndex, Card card)
		{
			this.playerIndex = playerIndex;
			this.cardIndex = cardIndex;
			this.card = card;
		}
	}
}
