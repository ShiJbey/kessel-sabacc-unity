using System.Collections;
using System.Threading.Tasks;
using KesselSabacc.Model;
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
			Debug.Log( $"Staring Round {_gameController.Model.CurrentRound}" );
			yield return null;
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
			if ( !_gameController.Model.Players[_gameController.Model.CurrentTurnTaker].IsDisqualified )
			{
				_gameController.Players[_gameController.Model.CurrentTurnTaker].TakeTurn( _gameController.Model );
			}

			_gameController.Model.IncrementTurnTaker();


			if ( _gameController.Model.CurrentTurn >= KesselSabaccGameModel.TURNS_PER_ROUND )
			{
				_gameController.GoToRoundOverState();
			}
		}
	}
}
