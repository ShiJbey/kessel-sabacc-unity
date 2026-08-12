namespace KesselSabacc.Model.PlayerActions
{
	/// <summary>
	/// The performer choses not to draw any cards this turn.
	/// </summary>
	public class StandAction : PlayerAction
	{
		public readonly int PlayerIndex;

		public override ActionType ActionType => ActionType.STAND;

		public StandAction(int playerIndex)
		{
			PlayerIndex = playerIndex;
		}

		public override void Execute(KesselSabaccGameModel model)
		{
			Player player = model.Players[PlayerIndex];
			player.HasStoodThisTurn = true;
		}
	}
}
