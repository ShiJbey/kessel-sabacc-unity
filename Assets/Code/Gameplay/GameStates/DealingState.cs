using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class DealingState : GameState
	{
		public override async Awaitable OnEnter(KesselSabaccGameController gameController)
		{
			// gameController.PlayDealingSequence();
			gameController.Model.ClearHands();
			gameController.Model.ClearDiscardPiles();
			gameController.Model.ClearDrawPiles();
			await gameController.CommandSystem.WaitUntilIdle();
			gameController.Model.ResetDrawPiles();
			await gameController.CommandSystem.WaitUntilIdle();
			gameController.Model.DealHands();
			await gameController.CommandSystem.WaitUntilIdle();
			gameController.GoToTurnTakingState();
		}
	}
}
