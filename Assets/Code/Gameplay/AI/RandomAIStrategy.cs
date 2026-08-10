using System.Collections.Generic;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Gameplay.AI
{
	[CreateAssetMenu(fileName = "RandomStrategy", menuName = "Kessel Sabacc/AI/Random Strategy")]
	public class RandomAIStrategy : AIStrategy
	{
		public override async Awaitable<PlayerAction> SelectAction(PlayerController player, IReadOnlyList<PlayerAction> legalActions)
		{
			await Awaitable.WaitForSecondsAsync(1.5f);
			return legalActions[Random.Range( 0, legalActions.Count )];
		}
	}
}
