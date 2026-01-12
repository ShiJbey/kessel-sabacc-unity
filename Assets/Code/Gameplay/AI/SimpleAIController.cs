using System.Collections;
using System.Collections.Generic;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Gameplay.AI
{
	public class SimpleAIController : PlayerController
	{
		public SimpleAIController(Player model) : base( model )
		{
		}

		public List<PlayerAction> GetActions()
		{
			return new List<PlayerAction>();
		}

		public PlayerAction SelectAction(IReadOnlyList<PlayerAction> actions)
		{
			return actions[Random.Range( 0, actions.Count )];
		}

		public override IEnumerator TakeTurn(KesselSabaccGameController gameController)
		{
			List<PlayerAction> availableActionList = GetActions();

			while ( availableActionList.Count > 0 )
			{
				PlayerAction selectedAction = SelectAction( availableActionList );
				yield return selectedAction.Execute( gameController );
				availableActionList = GetActions();
			}
		}
	}
}
