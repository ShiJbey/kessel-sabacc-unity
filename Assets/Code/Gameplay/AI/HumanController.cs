using System.Collections;
using KesselSabacc.Model;
using KesselSabacc.Views;
using UnityEngine;

namespace KesselSabacc.Gameplay.AI
{
	public class HumanController : PlayerController
	{
		private KesselSabaccGameView _gameView;
		private KesselSabaccGameController _gameController;
		private bool _isTakingTurn;

		public HumanController(Player model) : base( model )
		{
		}

		public override void Initialize(KesselSabaccGameController gameController)
		{
			// Add listeners
			_gameController = gameController;
			_gameView = gameController.uiView;
			_gameView.hud.OnDrawCardButtonClicked += GameView_OnDrawCardButtonClicked;
			_gameView.hud.OnStandButtonClicked += GameView_OnStandButtonClicked;
			_gameView.drawCardUI.OnCardDrawn += GameView_OnCardDrawn;
			_gameView.discardCardUI.OnCardDiscarded += GameView_OnCardDiscarded;
		}

		public override IEnumerator TakeTurn(KesselSabaccGameController gameController)
		{
			_isTakingTurn = true;
			_gameView.hud.ShowActionButtons();


			yield return new WaitUntil( () => !_isTakingTurn );
			_gameView.hud.HideActionButtons();
		}

		private void GameView_OnDrawCardButtonClicked()
		{
			_gameView.drawCardUI.UpdateView( _gameView );
			_gameView.drawCardUI.Show();
		}

		private void GameView_OnStandButtonClicked()
		{
			_isTakingTurn = false;
		}

		private void GameView_OnCardDrawn(int choiceIndex)
		{
			_gameView.diceRollUI.Hide();

			CardView selectedCardView = null;

			switch ( choiceIndex )
			{
				case 0:
					selectedCardView = _gameView.tableView.SandDiscardPileView.Pop();
					break;
				case 1:
					selectedCardView = _gameView.tableView.SandDeckView.Pop();
					break;
				case 2:
					selectedCardView = _gameView.tableView.BloodDeckView.Pop();
					break;
				case 3:
					selectedCardView = _gameView.tableView.BloodDiscardPileView.Pop();
					break;
			}

			if ( selectedCardView == null ) return;

			_gameView.tableView.playerHands[0].AddCard( selectedCardView );

			var playerHand = _gameController.Model.Players[0].Hand;
			playerHand.drawnCard = selectedCardView.Card;

			if ( playerHand.drawnCard.Suit == CardSuit.BLOOD )
			{
				var cards = new Card[2] { playerHand.sandCard, playerHand.drawnCard };
				_gameView.discardCardUI.UpdateView( cards );
			}
			else
			{
				var cards = new Card[2] { playerHand.sandCard, playerHand.drawnCard };
				_gameView.discardCardUI.UpdateView( cards );
			}

			_gameView.discardCardUI.Show();



		}

		private void GameView_OnCardDiscarded(int choiceIndex)
		{
			_gameView.discardCardUI.Hide();
			_isTakingTurn = false;
		}
	}
}
