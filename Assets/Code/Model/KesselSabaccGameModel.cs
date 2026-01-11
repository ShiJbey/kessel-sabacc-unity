using System;
using System.Collections.Generic;

namespace KesselSabacc.Model
{
	public class KesselSabaccGameModel
	{
		public const int MIN_PLAYERS = 2;
		public const int MAX_PLAYERS = 4;
		public const int TURNS_PER_ROUND = 3;

		private List<Player> _players;
		private int _currentRound;
		private int _currentTurn;
		private int _currentTurnTaker;
		private int _playerWhoStartedTurn;
		private CardStack _sandDeck;
		private CardStack _bloodDeck;
		private CardStack _sandDiscardPile;
		private CardStack _bloodDiscardPile;

		public IReadOnlyList<Player> Players => _players;
		public int CurrentRound => _currentRound;
		public int CurrentTurn => _currentTurn;
		public int CurrentTurnTaker => _currentTurnTaker;
		public int PlayerWhoStartedTurn => _playerWhoStartedTurn;
		public CardStack SandDeck => _sandDeck;
		public CardStack BloodDeck => _bloodDeck;
		public CardStack SandDiscardPile => _sandDiscardPile;
		public CardStack BloodDiscardPile => _bloodDiscardPile;
		public bool IsGameOver { get; private set; }
		public bool IsRoundOver { get; private set; }

		public event Action<Player> OnPlayerAdded;
		public event Action<int> OnRoundStart;
		public event Action<int> OnRoundEnd;

		public KesselSabaccGameModel()
		{
			_players = new List<Player>();
			_currentRound = 1;
			_currentTurn = 1;
			_playerWhoStartedTurn = 0;
			_sandDeck = new CardStack( true );
			_bloodDeck = new CardStack( true );
			_sandDiscardPile = new CardStack( false );
			_bloodDiscardPile = new CardStack( false );
		}

		public void AddPlayer(Player player)
		{
			_players.Add( player );
			OnPlayerAdded?.Invoke( player );
		}


		/// <summary>
		/// Reset Blood and Sand decks.
		/// </summary>
		public void ResetDecks()
		{

		}

		public void DealToPlayer(int playerIndex)
		{
			// m_Players[playerIndex].Cards;
		}

		public int IncrementTurn()
		{
			return -1;
		}

		public int IncrementRound()
		{
			_currentRound += 1;
			return _currentRound;
		}

		public int IncrementTurnTaker()
		{
			_currentTurnTaker += 1;

			if ( _currentTurnTaker >= _players.Count )
			{
				_currentTurn += 1;
				_currentTurnTaker = 0;

			}

			return _currentTurnTaker;
		}

		/// <summary>
		/// Return true if the match is at the end of a round.
		/// </summary>
		/// <returns></returns>
		public bool IsRoundEnd()
		{
			return false;
		}

		public int[] GetHandScores()
		{
			return new int[0];
		}

		public int ScoreHand(Player player)
		{

			return -1;
		}

		public void RevealHands()
		{
			// This methos sgould be implemented by a monobehavior
			// We start from the player who started the round and continue
			// clockwise.
		}

		public void DetermineWinner()
		{
			// This method should belong to a MonoBehaviour controller script.
			// In his method, we take the scores falculated during haad reveals
			// and determine the winner.
		}

		public void ExecuteChipPenalties()
		{
			// Tax players' chips according to their final hands.
		}

		private void ResetPlayers()
		{
			foreach ( var p in _players )
			{
				p.ResetForNewRound();
			}
		}
	}
}
