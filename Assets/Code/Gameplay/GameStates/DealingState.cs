using System.Collections;
using KesselSabacc.Model;
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
			_gameController.ResetDecksAndPiles();
			Debug.Log( "Dealing Cards" );

			var sandDeckCoroutine = _gameController.StartCoroutine( _tableView.SandDeckView.AnimateDeckSpawn() );
			var bloodDeckCoroutine = _gameController.StartCoroutine( _tableView.BloodDeckView.AnimateDeckSpawn() );
			yield return null; // Give the above coroutines a chance to start

			yield return new WaitUntil( () => !_tableView.SandDeckView.IsAnimating && !_tableView.BloodDeckView.IsAnimating );
			yield return new WaitForSeconds( .500f );

			CardView sandCard = _tableView.SandDeckView.Pop();
			CardView bloodCard = _tableView.BloodDeckView.Pop();

			yield return sandCard.Flip();
			yield return bloodCard.Flip();

			_gameController.StartCoroutine( _tableView.SandDiscardPileView.AddCard( sandCard ) );
			_gameController.StartCoroutine( _tableView.BloodDiscardPileView.AddCard( bloodCard ) );
			yield return null; // Give the above coroutines a chance to start

			yield return new WaitUntil( () => !_tableView.SandDiscardPileView.IsAnimating && !_tableView.BloodDiscardPileView.IsAnimating );

			yield return new WaitForSeconds( .500f );

			for ( int i = 0; i < _gameController.Model.Players.Count; i++ )
			{
				var player = _gameController.Model.Players[i];
				if ( player.IsDisqualified ) continue;
				yield return DealCardsToPlayer( i );
				yield return new WaitForSeconds( .500f );
			}

			_gameController.uiView.roundNotificationUI.SetRound( _gameController.Model.CurrentRound );
			_gameController.uiView.roundNotificationUI.Show();

			yield return new WaitForSeconds( 2f );

			_gameController.uiView.roundNotificationUI.Hide();
		}

		public IEnumerator OnExit()
		{
			yield return null;
		}

		public void OnInput()
		{

		}

		IEnumerator DealCardsToPlayer(int playerIndex)
		{
			Debug.Log( "Dealing to player: " + playerIndex );

			CardView sandCardView = _tableView.SandDeckView.Pop();
			CardView bloodCardView = _tableView.BloodDeckView.Pop();

			Card sandCard = _gameController.Model.SandDeck.Pop();
			Card bloodCard = _gameController.Model.BloodDeck.Pop();

			_gameController.Model.Players[playerIndex].Hand.bloodCard = bloodCard;
			_gameController.Model.Players[playerIndex].Hand.sandCard = sandCard;

			yield return _tableView.playerHands[playerIndex].AddCard( sandCardView );
			if ( playerIndex == 0 )
			{
				yield return sandCardView.ShowFrontAsync();
			}

			yield return _tableView.playerHands[playerIndex].AddCard( bloodCardView );
			if ( playerIndex == 0 )
			{
				yield return bloodCardView.ShowFrontAsync();
			}
		}

		public void OnUpdate()
		{

		}
	}
}
