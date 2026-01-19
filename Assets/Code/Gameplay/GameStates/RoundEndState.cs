using System;
using System.Collections;
using System.Collections.Generic;
using KesselSabacc.Model;
using KesselSabacc.Views;
using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class RoundOverState : IGameState
	{
		private KesselSabaccGameController _gameController;

		private List<PlayerRoundResult> _playerResults = new();

		public RoundOverState(KesselSabaccGameController gameController)
		{
			_gameController = gameController;
		}

		public IEnumerator OnEnter()
		{
			_playerResults.Clear();

			_gameController.uiView.roundEndUI.OnNextButtonClicked += OnNextButtonClicked;
			Debug.Log( $"Ending Round {_gameController.Model.CurrentRound}" );
			_gameController.uiView.roundNotificationUI.ShowRoundEndMessage( _gameController.Model.CurrentRound );
			_gameController.uiView.roundNotificationUI.Show();
			yield return new WaitForSeconds( 2f );

			_gameController.uiView.roundNotificationUI.Hide();

			yield return RevealHandsAnimation();

			_gameController.uiView.roundEndUI.ClearScores();
			_gameController.uiView.roundEndUI.HideContinueButton();
			_gameController.uiView.roundEndUI.Show();
			yield return null;

			for ( int i = 0; i < _gameController.Players.Count; i++ )
			{
				PlayerController playerController = _gameController.Players[i];
				if ( playerController.Model.IsDisqualified ) continue;

				yield return RollImposterCards( playerController );

				var bloodCard = playerController.Model.GetFirstCardOfSuit( Model.CardSuit.BLOOD );
				var sandCard = playerController.Model.GetFirstCardOfSuit( Model.CardSuit.SAND );

				// Assign Sylop Card values
				if ( bloodCard.CardType == CardType.SYLOP ) bloodCard.SetValue( sandCard.Value );
				if ( sandCard.CardType == CardType.SYLOP ) sandCard.SetValue( bloodCard.Value );

				PlayerRoundResult roundResult = new PlayerRoundResult()
				{
					Player = playerController.Model,
					PlayerIndex = playerController.PlayerIndex,
					HandDifference = Math.Abs( bloodCard.Value - sandCard.Value ),
					HandSize = Math.Abs( bloodCard.Value + sandCard.Value ),
					HasPrimeSabacc = HandScoreUtils.HasPrimeSabaccHand( playerController.Model ),
					HasSabacc = HandScoreUtils.HasSabaccHand( playerController.Model ),
					PerformanceScore = HandScoreUtils.GetPerformanceScore( playerController.Model ),
				};

				_playerResults.Add( roundResult );

				yield return _gameController.uiView.roundEndUI.AddScore(
					playerController, roundResult.HandDifference );

				yield return new WaitForSeconds( 0.5f );
			}

			_gameController.uiView.roundEndUI.ShowContinueButton();


			for ( int i = 0; i < _gameController.Players.Count; i++ )
			{
				PlayerController playerController = _gameController.Players[i];
				if ( playerController.Model.IsDisqualified ) continue;

				var bloodCard = playerController.Model.GetFirstCardOfSuit( Model.CardSuit.BLOOD );
				var sandCard = playerController.Model.GetFirstCardOfSuit( Model.CardSuit.SAND );
				int score = Math.Abs( bloodCard.Value - sandCard.Value );

				playerController.Model.Chips = Math.Max(
					0,
					playerController.Model.Chips + (playerController.Model.ChipsInvested - score)
				);

				playerController.Model.ChipsInvested = 0;

				if ( playerController.Model.Chips == 0 )
				{
					playerController.Model.DisqualifyPlayer();
				}
			}
		}

		public IEnumerator RollImposterCards(PlayerController playerController)
		{
			var sandCard = playerController.Model.GetFirstCardOfSuit( CardSuit.SAND );
			if ( sandCard.CardType == CardType.IMPOSTER && !sandCard.IsValueModified() )
			{
				yield return playerController.AssignImposterValue( _gameController, sandCard );
			}

			var bloodCard = playerController.Model.GetFirstCardOfSuit( CardSuit.BLOOD );
			if ( bloodCard.CardType == CardType.IMPOSTER && !bloodCard.IsValueModified() )
			{
				yield return playerController.AssignImposterValue( _gameController, bloodCard );
			}
		}

		public IEnumerator OnExit()
		{
			_gameController.uiView.roundEndUI.OnNextButtonClicked -= OnNextButtonClicked;
			yield return null;
		}

		public void OnInput()
		{

		}

		public void OnUpdate()
		{

		}

		private void OnNextButtonClicked()
		{
			_gameController.uiView.roundEndUI.Hide();
			if ( _gameController.Model.IsGameOver )
			{
				_gameController.GoToRoundOverState();
			}
			else
			{
				_gameController.GoToDealingState();
			}
		}

		private IEnumerator RevealHandsAnimation()
		{
			foreach ( HandView handView in _gameController.uiView.tableView.playerHands )
			{
				foreach ( CardView cardView in handView.Cards )
				{
					yield return cardView.ShowFrontAsync();
				}
				yield return new WaitForSeconds( 1f );
			}

		}
	}
}
