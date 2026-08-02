using UnityEngine;
using KesselSabacc.Model;

namespace KesselSabacc.Gameplay.PlayerActions
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

		public override async Awaitable Execute(KesselSabaccGameController gameController)
		{
			await gameController.DealCardToPlayer( CardStack, PlayerIndex );
			Player player = gameController.Model.Players[PlayerIndex];
			player.Chips -= 1;
			player.ChipsInvested += 1;
			player.DrewCardThisTurn = true;
		}
	}
}
