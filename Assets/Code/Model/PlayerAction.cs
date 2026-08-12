namespace KesselSabacc.Model
{
	/// <summary>
	/// An action performed by the player that updates the state of the game.
	/// </summary>
	public abstract class PlayerAction
	{
		public abstract ActionType ActionType { get; }
		public abstract void Execute(KesselSabaccGameModel model);
	}
}
