using System.Collections.Generic;
using System.Threading.Tasks;
using KesselSabacc.Model;

namespace KesselSabacc.Gameplay.AI
{
	public class SimpleAIController : PlayerController
	{
		public SimpleAIController(Player model) : base( model )
		{
		}

		public override Task<PlayerAction> SelectAction(IReadOnlyList<PlayerAction> actions)
		{
			throw new System.NotImplementedException();
		}

		public override Task TakeTurn(KesselSabaccGameModel game)
		{
			throw new System.NotImplementedException();
		}
	}
}
