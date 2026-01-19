using System.Collections;
using KesselSabacc.Model;

namespace KesselSabacc.Gameplay
{
	public abstract class PlayerController
	{
		public Player Model { get; }
		public int PlayerIndex { get; }
		public bool IsTakingTurn { get; set; }

		public PlayerController(int playerIndex, Player model)
		{
			PlayerIndex = playerIndex;
			Model = model;
		}

		public virtual void Initialize(KesselSabaccGameController gameController) { }

		// /// <summary>
		// /// Select an action to perform from among those given.
		// /// </summary>
		// /// <param name="actions"></param>
		// /// <returns></returns>
		// public abstract Task<PlayerAction> SelectAction(IReadOnlyList<PlayerAction> actions);

		public abstract IEnumerator TakeTurn(KesselSabaccGameController gameController);

		public abstract IEnumerator AssignImposterValue(KesselSabaccGameController gameController, Card card);
	}
}
