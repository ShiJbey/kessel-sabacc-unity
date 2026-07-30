using System;
using KesselSabacc.Model;
using KesselSabacc.Views;
using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class RoundOverState : GameState
	{
		public override async Awaitable OnEnter(KesselSabaccGameController gameController)
		{
			gameController.Model.RoundResults.Clear();

			await gameController.uiView.roundNotificationUI.PlayRoundEndAnim(
				gameController.Model.CurrentRound
			);

			await RevealHandsAnimation( gameController );

			gameController.uiView.roundEndUI.ClearScores();
			gameController.uiView.roundEndUI.HideContinueButton();
			gameController.uiView.roundEndUI.Show();
			await Awaitable.NextFrameAsync();

			for ( int i = 0; i < gameController.Players.Count; i++ )
			{
				PlayerController playerController = gameController.Players[i];
				if ( playerController.Model.IsDisqualified ) continue;

				await RollImposterCards( gameController, playerController );

				var bloodCard = playerController.Model.GetFirstCardOfSuit( CardSuit.BLOOD );
				var sandCard = playerController.Model.GetFirstCardOfSuit( CardSuit.SAND );

				// Assign Sylop Card values
				if ( bloodCard.CardType == CardType.SYLOP ) bloodCard.SetValue( sandCard.Value );
				if ( sandCard.CardType == CardType.SYLOP ) sandCard.SetValue( bloodCard.Value );

				PlayerRoundResult roundResult = HandScoreUtils.CreateRoundResult(
					playerController.Model, playerController.PlayerIndex
				);

				gameController.Model.RoundResults.Add( roundResult );

				await Awaitable.WaitForSecondsAsync( 0.5f );
			}

			gameController.Model.RoundResults.Sort();
			var bestResult = gameController.Model.RoundResults.Results[0];

			foreach ( PlayerRoundResult roundResult in gameController.Model.RoundResults.Results )
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

			gameController.uiView.roundEndUI.ShowContinueButton();
		}

		public async Awaitable RollImposterCards(KesselSabaccGameController gameController, PlayerController playerController)
		{
			var sandCard = playerController.Model.GetFirstCardOfSuit( CardSuit.SAND );
			if ( sandCard.CardType == CardType.IMPOSTER && !sandCard.IsValueModified() )
			{
				await playerController.AssignImposterValue( gameController, sandCard );
			}

			var bloodCard = playerController.Model.GetFirstCardOfSuit( CardSuit.BLOOD );
			if ( bloodCard.CardType == CardType.IMPOSTER && !bloodCard.IsValueModified() )
			{
				await playerController.AssignImposterValue( gameController, bloodCard );
			}
		}

		public override void OnRoundAdvanced(KesselSabaccGameController gameController)
		{
			gameController.uiView.roundEndUI.Hide();
			if ( gameController.Model.IsGameOver() )
			{
				gameController.GoToGameOverState();
			}
			else
			{
				gameController.GoToDealingState();
			}
		}

		private async Awaitable RevealHandsAnimation(KesselSabaccGameController gameController)
		{
			foreach ( HandView handView in gameController.uiView.tableView.playerHands )
			{
				foreach ( CardView cardView in handView.Cards )
				{
					await cardView.ShowFrontAsync();
				}
				await Awaitable.WaitForSecondsAsync( 1f );
			}

		}
	}
}
