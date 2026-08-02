using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Gameplay.PlayerActions
{
	/// <summary>
	/// The performer choses not to draw any cards this turn.
	/// </summary>
	public class StandAction : PlayerAction
	{
		public readonly int PlayerIndex;

		public override ActionType ActionType => ActionType.STAND;

		public StandAction(int playerIndex)
		{
			PlayerIndex = playerIndex;
		}

		public override async Awaitable Execute(KesselSabaccGameController gameController)
		{
			Player player = gameController.Model.Players[PlayerIndex];
			player.HasStoodThisTurn = true;
			gameController.Model.IsPlayerTurnOver = true;
			await Awaitable.NextFrameAsync();
		}
	}
}
