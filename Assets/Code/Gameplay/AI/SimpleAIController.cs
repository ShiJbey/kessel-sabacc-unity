using System.Collections.Generic;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Gameplay.AI
{
	public class SimpleAIController : PlayerController
	{
		public override async Awaitable AssignImposterValue(KesselSabaccGameController gameController, Card card)
		{
			card.SetValue( UnityEngine.Random.Range( 1, 6 ) );
			await Awaitable.WaitForSecondsAsync( 1f );
		}

		protected override async Awaitable<PlayerAction> SelectAction(KesselSabaccGameController gameController, IReadOnlyList<PlayerAction> legalActions)
		{
			await Awaitable.WaitForSecondsAsync(1.5f);
			return legalActions[Random.Range( 0, legalActions.Count )];
		}
	}
}
