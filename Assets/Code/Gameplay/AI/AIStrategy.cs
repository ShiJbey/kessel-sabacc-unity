using System.Collections.Generic;
using UnityEngine;

namespace KesselSabacc.Gameplay.AI
{
	public abstract class AIStrategy : ScriptableObject
	{
		public abstract Awaitable<PlayerAction> SelectAction(
			PlayerController player,
			IReadOnlyList<PlayerAction> legalActions);
	}
}
