using System.Collections;
using KesselSabacc.Views;
using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class DealingState : IGameState
	{
		private KesselSabaccGameController _gameController;

		public DealingState(KesselSabaccGameController gameController)
		{
			_gameController = gameController;
		}

		public IEnumerator OnEnter()
		{
			_gameController.ClearHands();
			yield return _gameController.PlayDealingSequence();
			_gameController.GoToTurnTakingState();
		}

		public IEnumerator OnExit()
		{
			yield return null;
		}

		public void OnInput()
		{

		}

		public void OnUpdate()
		{

		}
	}
}
