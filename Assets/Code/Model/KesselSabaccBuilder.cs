namespace KesselSabacc.Model
{
	public class KesselSabaccBuilder
	{
		private int _numPlayers;
		private int _startingChips;

		public KesselSabaccBuilder WithNumPlayers(int numPlayers)
		{
			if ( numPlayers < 2 || numPlayers > 4 )
			{
				throw new System.ArgumentException( $"Sabacc games may have 2 - 4 players. Given: {numPlayers}" );
			}
			_numPlayers = numPlayers;
			return this;
		}

		public KesselSabaccBuilder WithStartingChips(int startingChips)
		{
			if ( startingChips < 4 || startingChips > 8 )
			{
				throw new System.ArgumentException( $"Sabacc use 4 - 8 chips. Given: {startingChips}" );
			}
			_startingChips = startingChips;
			return this;
		}

		public KesselSabacc Build()
		{
			return new KesselSabacc();
		}
	}
}
