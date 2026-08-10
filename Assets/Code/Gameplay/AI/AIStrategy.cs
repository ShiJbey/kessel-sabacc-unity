using System.Collections.Generic;
using KesselSabacc.Model;
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
