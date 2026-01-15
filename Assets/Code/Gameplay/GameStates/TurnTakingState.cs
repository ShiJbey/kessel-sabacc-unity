using System.Collections;
using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class TurnTakingState : IGameState
	{
		private KesselSabaccGameController _gameController;

		public TurnTakingState(KesselSabaccGameController gameController)
		{
			_gameController = gameController;
		}

		public IEnumerator OnEnter()
		{
			_gameController.AdvanceRound();

			Debug.Log( $"Staring Round {_gameController.Model.CurrentRound}" );
			_gameController.uiView.roundNotificationUI.ShowRoundStartMessage( _gameController.Model.CurrentRound );
			_gameController.uiView.roundNotificationUI.Show();

			yield return new WaitForSeconds( 2f );

			_gameController.uiView.roundNotificationUI.Hide();

			yield return null;

			while ( !_gameController.Model.IsRoundOver )
			{
				while ( !_gameController.Model.IsTurnOver )
				{
					int playerIndex = _gameController.Model.CurrentTurnTaker;
					PlayerController playerController = _gameController.Players[playerIndex];
					if ( !playerController.Model.IsDisqualified )
					{
						yield return playerController.TakeTurn( _gameController );
					}
					_gameController.AdvanceTurnTaker();
				}

				_gameController.AdvanceTurn();
			}

			_gameController.GoToRoundOverState();
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
