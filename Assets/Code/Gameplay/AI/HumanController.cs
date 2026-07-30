using System.Collections.Generic;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Gameplay.AI
{
	public class HumanController : PlayerController
	{
		private int _dieValue;

		private AwaitableCompletionSource<PlayerAction> _pendingSelection;

		protected override Awaitable<PlayerAction> SelectAction(
			KesselSabaccGameController gameController, IReadOnlyList<PlayerAction> legalActions)
		{
			_pendingSelection = new AwaitableCompletionSource<PlayerAction>();

			gameController.uiView.PresentActionUI( legalActions, OnActionChosen );

			return _pendingSelection.Awaitable;
		}

		private void OnActionChosen(PlayerAction action)
		{
			GameController.uiView.HideAllActionUI();
			_pendingSelection.SetResult( action );
			_pendingSelection = null;
		}

		private void HandleDieSelected(int value)
		{
			_dieValue = value;
		}

		public override async Awaitable AssignImposterValue(KesselSabaccGameController gameController, Card card)
		{
			card.SetValue( UnityEngine.Random.Range( 1, 6 ) );
			await Awaitable.WaitForSecondsAsync( 1f );

			// _gameView.diceRollUI.Show();
			// _gameView.diceRollUI.OnDieResult += HandleDieSelected;

			// await Awaitable.FromAsyncOperation WaitUntil( () => _dieValue > 0 );

			// _gameView.diceRollUI.OnDieResult -= HandleDieSelected;

			// yield return new WaitUntil( () => !IsTakingTurn );

			// card.SetValue( _dieValue );
			// _gameView.diceRollUI.Hide();
			// _dieValue = -1;
		}
	}
}
