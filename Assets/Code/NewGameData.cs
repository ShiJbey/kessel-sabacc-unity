using System;

namespace KesselSabacc
{
	[Serializable]
	public class NewGameData
	{
		public string playerName = "";
		public int numChips = 4;
		public bool shiftTokensEnabled = true;
		public int numPlayers = 4;
	}
}
