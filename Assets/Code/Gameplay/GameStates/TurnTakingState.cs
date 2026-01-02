using System.Threading.Tasks;
using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class TurnTakingState : IGameState
	{
		private KesselSabaccController _gameController;

		public Task OnEnter()
		{
			_gameController = GameplayManager.Instance.GameController;
			// _gameController.Model.CurrentTurn = 0;

			Debug.Log( $"Staring Round {_gameController.Model.CurrentRound}" );
			return Task.CompletedTask;
		}

		public Task OnExit()
		{
			return Task.CompletedTask;
		}

		public void OnInput()
		{

		}

		public void OnUpdate()
		{
			if ( !_gameController.Model.Players[_gameController.Model.CurrentTurnTaker].IsDisqualified )
			{
				_gameController.Players[_gameController.Model.CurrentTurnTaker].TakeTurn( _gameController.Model );
			}

			_gameController.Model.IncrementTurnTaker();


			if ( _gameController.Model.CurrentTurn >= Model.KesselSabacc.TURNS_PER_ROUND )
			{
				GameplayManager.Instance.GoToRoundOverState();
			}
		}
	}
}
