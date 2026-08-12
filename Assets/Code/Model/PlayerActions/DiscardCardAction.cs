namespace KesselSabacc.Model.PlayerActions
{
	/// <summary>
	/// The performer discards a duplicate card from their hand.
	/// </summary>
	public class DiscardCardAction : PlayerAction
	{
		public readonly int playerIndex;
		public readonly int cardIndex;
		public readonly CardType cardType;
		public readonly CardSuit cardSuit;

		public override ActionType ActionType => ActionType.DISCARD_CARD;

		public DiscardCardAction(int playerIndex, int cardIndex, CardType cardType, CardSuit cardSuit)
		{
			this.playerIndex = playerIndex;
			this.cardIndex = cardIndex;
			this.cardType = cardType;
			this.cardSuit = cardSuit;
		}

		public override void Execute(KesselSabaccGameModel model)
		{
			model.DiscardCard(playerIndex, cardIndex);
		}
	}
}
