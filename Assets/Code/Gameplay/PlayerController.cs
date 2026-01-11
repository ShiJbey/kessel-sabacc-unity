using System.Collections.Generic;
using System.Threading.Tasks;
using KesselSabacc.Model;

namespace KesselSabacc.Gameplay
{
	public abstract class PlayerController
	{
		private Player _model;

		public Player Model => _model;

		public PlayerController(Player model)
		{
			_model = model;
		}

		/// <summary>
		/// Select an action to perform from among those given.
		/// </summary>
		/// <param name="actions"></param>
		/// <returns></returns>
		public abstract Task<PlayerAction> SelectAction(IReadOnlyList<PlayerAction> actions);

		public abstract Task TakeTurn(KesselSabaccGameModel game);
	}
}
