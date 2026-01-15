using System.Collections;
using KesselSabacc.Views;
using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class DealingState : IGameState
	{
		private KesselSabaccGameController _gameController;
		private TableView _tableView;

		public DealingState(KesselSabaccGameController gameController)
		{
			_gameController = gameController;
			_tableView = gameController.uiView.tableView;
		}

		public IEnumerator OnEnter()
		{
			Debug.Log( "Dealing Cards" );

			_gameController.ClearHands();
			yield return _gameController.ResetDecksAndPiles();
			yield return new WaitForSeconds( .500f );

			yield return _gameController.DiscardTopCardOfDeck(
				_tableView.SandDeckView, _tableView.SandDiscardPileView );

			yield return _gameController.DiscardTopCardOfDeck(
				_tableView.BloodDeckView, _tableView.BloodDiscardPileView );

			yield return new WaitForSeconds( .500f );

			for ( int i = 0; i < _gameController.Model.Players.Count; i++ )
			{
				var player = _gameController.Model.Players[i];
				if ( player.IsDisqualified ) continue;
				yield return _gameController.DealCardToPlayer( _tableView.SandDeckView, i );
				yield return _gameController.DealCardToPlayer( _tableView.BloodDeckView, i );
				yield return new WaitForSeconds( .500f );
			}

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
