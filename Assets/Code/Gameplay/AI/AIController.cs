using System.Collections.Generic;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Gameplay.AI
{
	public class AIController : PlayerController
	{
		public AIStrategy Strategy { get; set; }

		public override async Awaitable<int> PerformDiceRoll(KesselSabaccGameController gameController)
		{
			await Awaitable.WaitForSecondsAsync( 1f );
			return UnityEngine.Random.Range( 1, 6 );
		}

		protected override async Awaitable<PlayerAction> SelectAction(KesselSabaccGameController gameController, IReadOnlyList<PlayerAction> legalActions)
		{
			return await Strategy.SelectAction(this, legalActions);
		}
	}
}
