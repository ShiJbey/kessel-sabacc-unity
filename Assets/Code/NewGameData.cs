using System;

namespace LoveHina
{
	[Serializable]
	public struct NewGameData
	{
		public string playerName;
		public int startingChips;
		public int shiftTokensEnabled;
		public int numberOfPlayers;
	}
}
