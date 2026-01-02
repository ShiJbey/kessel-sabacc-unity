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

		/// <summary>
		/// Execute the action by updating the provided game state.
		/// </summary>
		/// <param name="sabacc"></param>
		/// <returns></returns>
		public abstract KesselSabacc Execute(KesselSabacc sabacc);
	}
}
