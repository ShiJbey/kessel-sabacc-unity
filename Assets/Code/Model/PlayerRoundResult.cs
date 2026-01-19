namespace KesselSabacc.Model
{
	/// <summary>
	/// Data about a player's performance at the end of a round.
	/// </summary>
	public class PlayerRoundResult
	{
		public int PlayerIndex { get; set; }
		public Player Player { get; set; }
		public int HandSize { get; set; }
		public int HandDifference { get; set; }
		public int PerformanceScore { get; set; }
		public bool HasPrimeSabacc { get; set; }
		public bool HasSabacc { get; set; }
	}
}
