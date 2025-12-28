namespace KesselSabacc.Model
{
	public class Player
	{
		private string _name;
		private int _chips;
		private int _chipsInvested;
		private Hand _hand;
		private bool _stoodThisRound;

		public Player()
		{
			_name = "";
			_chips = 0;
			_chipsInvested = 0;
			_hand = new Hand();
			_stoodThisRound = false;
		}
	}
}
