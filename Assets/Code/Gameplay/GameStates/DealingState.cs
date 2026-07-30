using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class DealingState : GameState
	{
		public override async Awaitable OnEnter(KesselSabaccGameController gameController)
		{
			gameController.ClearHands();
			await gameController.PlayDealingSequence();
			gameController.GoToTurnTakingState();
		}
	}
}
