using System;

namespace KesselSabacc
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
