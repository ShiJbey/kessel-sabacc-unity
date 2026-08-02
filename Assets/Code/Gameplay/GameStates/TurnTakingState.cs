using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class TurnTakingState : GameState
	{
		public override async Awaitable OnEnter(KesselSabaccGameController gameController)
		{
			await gameController.uiView.roundNotificationUI.PlayRoundStartAnim(
				gameController.Model.CurrentRound
			);

			// Start the first player turn of the round
			while ( !gameController.Model.IsRoundOver )
			{
				while ( !gameController.Model.IsTurnOver )
				{
					var player = gameController.Players[gameController.Model.CurrentTurnTaker];
					if ( !player.Model.IsDisqualified )
					{
						await player.TakeTurn( gameController );
					}
					gameController.AdvanceTurnTaker();
				}

				gameController.AdvanceTurn();
			}

			gameController.GoToRoundOverState();
		}
	}
}
