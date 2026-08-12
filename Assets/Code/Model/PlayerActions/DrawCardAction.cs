namespace KesselSabacc.Model.PlayerActions
{
	/// <summary>
	/// The performer draws a single card from one of the four piles.
	/// </summary>
	public class DrawCardAction : PlayerAction
	{
		public readonly int PlayerIndex;
		public readonly CardStack CardStack;
		public readonly Card Card;

		public override ActionType ActionType => ActionType.DRAW_CARD;

		public DrawCardAction(int playerIndex, Card card, CardStack cardStack)
		{
			PlayerIndex = playerIndex;
			CardStack = cardStack;
			Card = card;
		}

		public override void Execute(KesselSabaccGameModel model)
		{
			Player player = model.Players[PlayerIndex];
			player.Chips -= 1;
			player.ChipsInvested += 1;
			player.DrewCardThisTurn = true;
			model.DrawCard(PlayerIndex, CardStack);
		}
	}
}
