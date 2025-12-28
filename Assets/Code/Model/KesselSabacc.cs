using System;
using System.Collections.Generic;

namespace KesselSabacc.Model
{
	public class KesselSabacc
	{
		public const int MIN_PLAYERS = 2;
		public const int MAX_PLAYERS = 4;

		private List<Player> _players;
		private int _currentRound;
		private int _currentTurn;
		private int _playerWhoStartedTurn;
		private CardStack _sandDeck;
		private CardStack _bloodDeck;
		private CardStack _sandDiscardPile;
		private CardStack _bloodDiscardPile;

		public event Action<Player> OnPlayerAdded;

		public KesselSabacc()
		{
			_players = new List<Player>();
			_currentRound = 1;
			_currentTurn = 1;
			_playerWhoStartedTurn = 0;
			_sandDeck = new CardStack();
			_bloodDeck = new CardStack();
			_sandDiscardPile = new CardStack();
			_bloodDiscardPile = new CardStack();
		}

		public void AddPlayer(Player player)
		{
			_players.Add( player );
			OnPlayerAdded?.Invoke( player );
		}

	}
}
