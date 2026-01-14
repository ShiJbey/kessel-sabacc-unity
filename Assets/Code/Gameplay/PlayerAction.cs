using System.Collections;
using KesselSabacc.Model;

namespace KesselSabacc.Gameplay
{
	/// <summary>
	/// An action performed by the player that updates the state of the game.
	/// </summary>
	public abstract class PlayerAction
	{
		public PlayerController Performer { get; }

		public PlayerAction(PlayerController performer)
		{
			Performer = performer;
		}

		public abstract void ApplyToModel(KesselSabaccGameModel model);

		public abstract IEnumerator Execute(KesselSabaccGameController gameController);
	}
}
