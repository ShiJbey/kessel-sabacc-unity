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
		private int _dieValue;

		public HumanController(int playerIndex, Player model) : base( playerIndex, model ) { }

		public override void Initialize(KesselSabaccGameController gameController)
		{
			// Add listeners
			_gameController = gameController;
			_gameView = gameController.uiView;
			_gameView.hud.OnDrawCardButtonClicked += GameView_OnDrawCardButtonClicked;
			_gameView.hud.OnStandButtonClicked += GameView_OnStandButtonClicked;
			_gameView.drawCardUI.OnCardDrawn += GameView_OnCardDrawn;
			_gameView.discardCardUI.OnCardSelected += GameView_OnCardDiscarded;
		}

		public override IEnumerator TakeTurn(KesselSabaccGameController gameController)
		{
			IsTakingTurn = true;
			_gameView.hud.ShowActionButtons();


			yield return new WaitUntil( () => !IsTakingTurn );
			_gameView.hud.HideActionButtons();
		}

		private void GameView_OnDrawCardButtonClicked()
		{
			_gameView.drawCardUI.UpdateView( _gameView );
			_gameView.drawCardUI.Show();
		}

		private void GameView_OnStandButtonClicked()
		{
			IsTakingTurn = false;
		}

		private void GameView_OnCardDrawn(int choiceIndex)
		{
			_gameView.drawCardUI.Hide();
			Model.Chips -= 1;
			Model.ChipsInvested += 1;

			CardStackView selectedCardStack = null;

			switch ( choiceIndex )
			{
				case 0:
					selectedCardStack = _gameView.tableView.SandDiscardPileView;
					break;
				case 1:
					selectedCardStack = _gameView.tableView.SandDeckView;
					break;
				case 2:
					selectedCardStack = _gameView.tableView.BloodDeckView;
					break;
				case 3:
					selectedCardStack = _gameView.tableView.BloodDiscardPileView;
					break;
			}

			if ( selectedCardStack == null ) return;

			_gameController.StartCoroutine(
				_gameController.DealCardToPlayer( selectedCardStack, 0, (cardView) =>
				{
					if ( cardView.Card.Suit == CardSuit.BLOOD )
					{
						var bloodCards = Model.GetCardsOfSuit( CardSuit.BLOOD );
						var sandCard = Model.GetFirstCardOfSuit( CardSuit.SAND );
						_gameView.discardCardUI.UpdateView( bloodCards, sandCard );
					}
					else
					{
						var sandCards = Model.GetCardsOfSuit( CardSuit.SAND );
						var bloodCard = Model.GetFirstCardOfSuit( CardSuit.BLOOD );
						_gameView.discardCardUI.UpdateView( sandCards, bloodCard );
					}

					_gameView.discardCardUI.Show();
				} )
			);


		}

		private void GameView_OnCardDiscarded(Card card)
		{
			_gameView.discardCardUI.Hide();

			_gameController.StartCoroutine(
				_gameController.DiscardCardFromPlayer( 0, card, () =>
					{
						IsTakingTurn = false;
					}
				)
			);
		}

		private void HandleDieSelected(int value)
		{
			_dieValue = value;
			IsTakingTurn = false;
		}

		public override IEnumerator AssignImposterValue(KesselSabaccGameController gameController, Card card)
		{
			IsTakingTurn = true;

			_gameView.diceRollUI.Show();
			_gameView.diceRollUI.OnDieResult += HandleDieSelected;

			yield return new WaitUntil( () => _dieValue > 0 );

			_gameView.diceRollUI.OnDieResult -= HandleDieSelected;

			yield return new WaitUntil( () => !IsTakingTurn );

			card.SetValue( _dieValue );
			_gameView.diceRollUI.Hide();
			_dieValue = -1;
		}
	}
}
