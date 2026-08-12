using System;
using KesselSabacc.Model;
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

			gameController.uiView.tableView.RevealHands();

			await gameController.CommandSystem.WaitUntilIdle();

			gameController.uiView.roundEndUI.ClearScores();
			gameController.uiView.roundEndUI.HideContinueButton();
			gameController.uiView.roundEndUI.Show();
			await Awaitable.NextFrameAsync();

			for ( int i = 0; i < gameController.Players.Count; i++ )
			{
				PlayerController playerController = gameController.Players[i];
				if ( playerController.Model.IsDisqualified ) continue;

				PlayerRoundResult roundResult = new PlayerRoundResult()
				{
					Player = playerController.Model,
					PlayerIndex = playerController.PlayerIndex,
					HandScore = gameController.Model.GetPlayerScore(i),
					SandCard = playerController.Model.Hand.GetFirstCardOfSuit( CardSuit.SAND ),
					BloodCard = playerController.Model.Hand.GetFirstCardOfSuit( CardSuit.BLOOD ),
				};

				gameController.Model.RoundResults.Add( roundResult );

				await RollImposterCards( roundResult, gameController, playerController );

				var bloodCard = playerController.Model.GetFirstCardOfSuit( CardSuit.BLOOD );
				var sandCard = playerController.Model.GetFirstCardOfSuit( CardSuit.SAND );

				// Assign Sylop Card values
				if ( bloodCard.CardType == CardType.SYLOP ) bloodCard.SetValue( sandCard.Value );
				if ( sandCard.CardType == CardType.SYLOP ) sandCard.SetValue( bloodCard.Value );

				roundResult.HandScore = gameController.Model.GetPlayerScore(i);

				roundResult.Update();

				await Awaitable.WaitForSecondsAsync( 0.5f );
			}

			gameController.Model.RoundResults.Sort();

			await gameController.uiView.roundEndUI.SortRows();

			gameController.Model.ApplyRoundEndResults();

			gameController.uiView.roundEndUI.ShowContinueButton();
		}

		public async Awaitable RollImposterCards(PlayerRoundResult result,KesselSabaccGameController gameController, PlayerController playerController)
		{
			var sandCard = playerController.Model.GetFirstCardOfSuit( CardSuit.SAND );
			if ( sandCard.CardType == CardType.IMPOSTER && !sandCard.IsValueModified() )
			{
				playerController.Model.IsRolling = true;
				int value = await playerController.PerformDiceRoll( gameController );
				result.SandCard.SetValue(value);
				playerController.Model.IsRolling = false;
			}

			var bloodCard = playerController.Model.GetFirstCardOfSuit( CardSuit.BLOOD );
			if ( bloodCard.CardType == CardType.IMPOSTER && !bloodCard.IsValueModified() )
			{
				playerController.Model.IsRolling = true;
				int value = await playerController.PerformDiceRoll( gameController );
				result.BloodCard.SetValue(value);
				playerController.Model.IsRolling = false;
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
	}
}
