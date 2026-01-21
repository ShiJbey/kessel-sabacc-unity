using System;

namespace KesselSabacc.Model
{
	/// <summary>
	/// Data about a player's performance at the end of a round.
	/// </summary>
	public class PlayerRoundResult : IComparable<PlayerRoundResult>
	{
		public int PlayerIndex { get; set; }
		public Player Player { get; set; }
		public Card SandCard { get; set; }
		public Card BloodCard { get; set; }
		public int HandSize { get; set; }
		public int HandDifference { get; set; }
		public int PerformanceScore { get; set; }
		public bool HasPrimeSabacc { get; set; }
		public bool HasSabacc { get; set; }
		public bool WonRound { get; set; }

		public int CompareTo(PlayerRoundResult other)
		{
			if ( HasPrimeSabacc && !other.HasPrimeSabacc ) return 1;

			if ( !HasPrimeSabacc && other.HasPrimeSabacc ) return -1;

			if ( HasSabacc && !other.HasSabacc ) return 1;

			if ( !HasSabacc && other.HasSabacc ) return -1;

			if ( HasSabacc && other.HasSabacc || HandDifference == other.HandDifference )
			{
				if ( HandSize > other.HandSize )
				{
					return -1;
				}
				else if ( HandSize < other.HandSize )
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
			else
			{
				if ( HandDifference < other.HandDifference )
				{
					return 1;
				}
				else if ( HandDifference > other.HandDifference )
				{
					return -1;
				}
				else
				{
					return 0;
				}
			}
		}
	}
}
