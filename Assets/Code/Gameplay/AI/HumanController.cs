using System.Collections.Generic;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Gameplay.AI
{
	public class HumanController : PlayerController
	{
		private AwaitableCompletionSource<PlayerAction> _pendingSelection;
		private AwaitableCompletionSource<int> _pendingDiceValue;

		protected override async Awaitable<PlayerAction> SelectAction(
			KesselSabaccGameController gameController, IReadOnlyList<PlayerAction> legalActions)
		{
			_pendingSelection = new AwaitableCompletionSource<PlayerAction>();

			gameController.uiView.PresentActionUI( legalActions, OnActionChosen );

			return await _pendingSelection.Awaitable;
		}

		private void OnActionChosen(PlayerAction action)
		{
			GameController.uiView.HideAllActionUI();
			_pendingSelection.SetResult( action );
			_pendingSelection = null;
		}

		private void OnDieResult(int value)
		{
			_pendingDiceValue.SetResult(value);
			_pendingDiceValue = null;
		}

		public override async Awaitable<int> PerformDiceRoll(KesselSabaccGameController gameController)
		{
			_pendingDiceValue = new AwaitableCompletionSource<int>();
			gameController.uiView.PresentDiceRoll(OnDieResult);
			return await _pendingDiceValue.Awaitable;
		}
	}
}
