using System.Collections.Generic;
using UnityEngine;

namespace KesselSabacc.Gameplay.AI
{
	[CreateAssetMenu(fileName = "UtilityStrategy", menuName = "Kessel Sabacc/AI/Utility Strategy")]
	public class UtilityAIStrategy : AIStrategy
	{
		public override async Awaitable<PlayerAction> SelectAction(PlayerController player, IReadOnlyList<PlayerAction> legalActions)
		{
			await Awaitable.WaitForSecondsAsync(1.5f);
			return legalActions[Random.Range(0, legalActions.Count)];
		}
	}
}
