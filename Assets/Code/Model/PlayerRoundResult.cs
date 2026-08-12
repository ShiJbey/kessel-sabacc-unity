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
		public int HandScore { get; set; }
		public bool WonRound { get; set; }

		public event Action OnResultUpdated;

		public void Update()
		{
			OnResultUpdated?.Invoke();
		}

		public int CompareTo(PlayerRoundResult other)
		{
			if (this.HandScore > other.HandScore)
			{
				return 1;
			}
			else if (this.HandScore == other.HandScore)
			{
				return 0;
			}
			else
			{
				return -1;
			}
		}
	}
}
