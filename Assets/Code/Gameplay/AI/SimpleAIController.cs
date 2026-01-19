using System.Collections;
using System.Collections.Generic;
using KesselSabacc.Gameplay.PlayerActions;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Gameplay.AI
{
	public class SimpleAIController : PlayerController
	{
		public SimpleAIController(int playerIndex, Player model) : base( playerIndex, model )
		{
		}

		public override IEnumerator AssignImposterValue(KesselSabaccGameController gameController, Card card)
		{
			card.SetValue( UnityEngine.Random.Range( 1, 6 ) );
			yield return new WaitForSeconds( 1f );
		}

		public List<PlayerAction> GetActions(KesselSabaccGameController gameController)
		{
			List<PlayerAction> availableActions = new();

			// Return an empty list.
			if ( Model.HasStoodThisTurn || !IsTakingTurn ) return availableActions;

			if ( Model.DrewCardThisTurn )
			{
				var sandCards = Model.GetCardsOfSuit( CardSuit.SAND );
				if ( sandCards.Length > 1 )
				{
					availableActions.Add( new DiscardCardAction( this, sandCards[0] ) );
					availableActions.Add( new DiscardCardAction( this, sandCards[1] ) );
				}

				var bloodCards = Model.GetCardsOfSuit( CardSuit.BLOOD );
				if ( bloodCards.Length > 1 )
				{
					availableActions.Add( new DiscardCardAction( this, bloodCards[0] ) );
					availableActions.Add( new DiscardCardAction( this, bloodCards[1] ) );
				}
			}
			else
			{
				if ( !gameController.Model.SandDiscardPile.IsEmpty() )
				{
					availableActions.Add(
						new DrawCardAction(
							this,
							gameController.Model.SandDiscardPile.Peek(),
							gameController.uiView.tableView.SandDiscardPileView
						)
					);
				}

				if ( !gameController.Model.SandDeck.IsEmpty() )
				{
					availableActions.Add(
						new DrawCardAction(
							this,
							gameController.Model.SandDeck.Peek(),
							gameController.uiView.tableView.SandDeckView
						)
					);
				}

				if ( !gameController.Model.BloodDiscardPile.IsEmpty() )
				{
					availableActions.Add(
						new DrawCardAction(
							this,
							gameController.Model.BloodDiscardPile.Peek(),
							gameController.uiView.tableView.BloodDiscardPileView
						)
					);
				}

				if ( !gameController.Model.BloodDeck.IsEmpty() )
				{
					availableActions.Add(
						new DrawCardAction(
							this,
							gameController.Model.BloodDeck.Peek(),
							gameController.uiView.tableView.BloodDeckView
						)
					);
				}


				availableActions.Add( new StandAction( this ) );
			}




			return availableActions;
		}

		public PlayerAction SelectAction(IReadOnlyList<PlayerAction> actions)
		{
			return actions[Random.Range( 0, actions.Count )];
		}

		public override IEnumerator TakeTurn(KesselSabaccGameController gameController)
		{
			IsTakingTurn = true;

			List<PlayerAction> availableActionList = GetActions( gameController );

			while ( availableActionList.Count > 0 )
			{
				PlayerAction selectedAction = SelectAction( availableActionList );
				yield return selectedAction.Execute( gameController );
				availableActionList = GetActions( gameController );
			}

			IsTakingTurn = false;
		}
	}
}
