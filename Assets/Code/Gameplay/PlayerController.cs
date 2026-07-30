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
			StartTurn();


			while ( !gameController.Model.IsPlayerTurnOver )
			{
				List<PlayerAction> legalActions = gameController.Model.GetLegalActions( PlayerIndex );

				if ( legalActions.Count == 0 )
					break;

				PlayerAction chosenAction = await SelectAction( gameController, legalActions );
				await chosenAction.Execute( gameController );
			}


			EndTurn();
		}

		protected abstract Awaitable<PlayerAction> SelectAction(
			KesselSabaccGameController gameController, IReadOnlyList<PlayerAction> legalActions);

		public abstract Awaitable AssignImposterValue(KesselSabaccGameController gameController, Card card);
	}
}
