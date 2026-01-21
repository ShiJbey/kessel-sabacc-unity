using System;
using System.Collections;
using KesselSabacc.Model;
using KesselSabacc.Views;
using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class RoundOverState : IGameState
	{
		private KesselSabaccGameController _gameController;

		public RoundOverState(KesselSabaccGameController gameController)
		{
			_gameController = gameController;
		}

		public IEnumerator OnEnter()
		{
			_gameController.Model.RoundResults.Clear();

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

				PlayerRoundResult roundResult = HandScoreUtils.CreateRoundResult(
					playerController.Model, playerController.PlayerIndex
				);

				_gameController.Model.RoundResults.Add( roundResult );

				yield return new WaitForSeconds( 0.5f );
			}

			_gameController.Model.RoundResults.Sort();
			var bestResult = _gameController.Model.RoundResults.Results[0];

			foreach ( PlayerRoundResult roundResult in _gameController.Model.RoundResults.Results )
			{
				roundResult.WonRound = roundResult == bestResult
					|| roundResult.CompareTo( bestResult ) == 0;

				if ( roundResult.WonRound )
				{
					// Winner is not taxed.
					roundResult.Player.Chips = Math.Max(
						0,
						roundResult.Player.Chips
						+ roundResult.Player.ChipsInvested
					);
				}
				else if ( roundResult.HasSabacc )
				{
					// Players that lose, but have sabacc are taxed one chip.
					roundResult.Player.Chips = Math.Max(
						0,
						roundResult.Player.Chips
						+ (roundResult.Player.ChipsInvested - 1)
					);
				}
				else
				{
					// Losers without sabacc are taxed the difference of their cards.
					roundResult.Player.Chips = Math.Max(
						0,
						roundResult.Player.Chips
						+ (roundResult.Player.ChipsInvested - roundResult.HandDifference)
					);
				}

				roundResult.Player.ChipsInvested = 0;

				if ( roundResult.Player.Chips == 0 )
				{
					roundResult.Player.DisqualifyPlayer();
				}
			}

			_gameController.uiView.roundEndUI.ShowContinueButton();
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
			if ( _gameController.Model.IsGameOver() )
			{
				_gameController.GoToGameOverState();
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
