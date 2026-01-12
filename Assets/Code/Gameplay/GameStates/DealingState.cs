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

			CardView sandCardView = _tableView.SandDeckView.Pop();
			Card sandCard = _gameController.Model.SandDeck.Pop();

			CardView bloodCardView = _tableView.BloodDeckView.Pop();
			Card bloodCard = _gameController.Model.BloodDeck.Pop();

			yield return sandCardView.Flip();
			yield return bloodCardView.Flip();

			_gameController.Model.SandDiscardPile.Add( sandCard );
			_gameController.Model.BloodDiscardPile.Add( bloodCard );

			yield return sandCardView.MoveToPosition( _tableView.SandDiscardPileView.transform.position );
			yield return bloodCardView.MoveToPosition( _tableView.BloodDiscardPileView.transform.position );

			yield return _tableView.SandDiscardPileView.AddCard( sandCardView );
			yield return _tableView.BloodDiscardPileView.AddCard( bloodCardView );
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

			_gameController.GoToTurnTakingState();
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
