using System;
using System.Collections.Generic;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Gameplay
{
	public abstract class PlayerController : MonoBehaviour
	{
		public Player Model { get; private set; }
		public int PlayerIndex { get; private set; }
		public bool IsTakingTurn { get; set; } = false;
		public KesselSabaccGameController GameController { get; private set; }

		public event Action OnTurnStarted;
		public event Action OnTurnEnded;
		public event Action OnThinkingStarted;
		public event Action OnThinkingEnded;

		public virtual void Initialize(int playerIndex, Player model, KesselSabaccGameController gameController)
		{
			PlayerIndex = playerIndex;
			Model = model;
			GameController = gameController;
		}

		public virtual void StartTurn() { }

		public virtual void EndTurn() { }

		public async Awaitable TakeTurn(KesselSabaccGameController gameController)
		{
			OnTurnStarted?.Invoke();
			StartTurn();
			await Awaitable.NextFrameAsync();

			while ( !gameController.Model.IsPlayerTurnOver )
			{
				List<PlayerAction> legalActions = gameController.Model.GetLegalActions( PlayerIndex );

				if ( legalActions.Count == 0 )
					break;

				OnThinkingStarted?.Invoke();
				await Awaitable.NextFrameAsync();

				PlayerAction chosenAction = await SelectAction( gameController, legalActions );

				OnThinkingEnded?.Invoke();
				await chosenAction.Execute( gameController );
			}

			OnTurnEnded?.Invoke();
			await Awaitable.NextFrameAsync();
			EndTurn();
		}

		protected abstract Awaitable<PlayerAction> SelectAction(
			KesselSabaccGameController gameController, IReadOnlyList<PlayerAction> legalActions);

		public abstract Awaitable<int> PerformDiceRoll(KesselSabaccGameController gameController);
	}
}
