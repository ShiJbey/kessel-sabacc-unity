using UnityEngine;
using KesselSabacc.Model;

namespace KesselSabacc.Gameplay.PlayerActions
{
	/// <summary>
	/// The performer discards a duplicate card from their hand.
	/// </summary>
	public class DiscardCardAction : PlayerAction
	{
		public readonly int PlayerIndex;
		public readonly Card Card;

		public override ActionType ActionType => ActionType.DISCARD_CARD;

		public DiscardCardAction(int playerIndex, Card card)
		{
			PlayerIndex = playerIndex;
			Card = card;
		}

		public override async Awaitable Execute(KesselSabaccGameController gameController)
		{
			await gameController.DiscardCardFromPlayer( PlayerIndex, Card );
			gameController.Model.IsPlayerTurnOver = true;
		}
	}
}
