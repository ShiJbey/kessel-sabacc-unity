using System.Collections;
using KesselSabacc.Gameplay;

namespace KesselSabacc.Model
{
	/// <summary>
	/// An action performed by the player that updates the state of the game.
	/// </summary>
	public abstract class PlayerAction
	{
		private Player _performer;

		public PlayerAction(Player performer)
		{
			_performer = performer;
		}

		public abstract void ApplyToModel(KesselSabaccGameModel model);

		public abstract IEnumerator Execute(KesselSabaccGameController gameController);
	}
}
