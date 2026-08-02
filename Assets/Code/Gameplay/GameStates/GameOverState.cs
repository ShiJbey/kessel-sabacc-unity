using System.Collections;
using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class GameOverState : GameState
	{
		public override async Awaitable OnEnter(KesselSabaccGameController gameController)
		{
			gameController.uiView.gameOverNotificationUI.ShowWinner(
				gameController.Model.GetWinner().Name
			);
			await Awaitable.NextFrameAsync();
		}
	}
}
