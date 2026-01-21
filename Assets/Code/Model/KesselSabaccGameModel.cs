using System;
using System.Collections.Generic;

namespace KesselSabacc.Model
{
	public class KesselSabaccGameModel
	{
		public const int TURNS_PER_ROUND = 3;

		private List<Player> _players;

		public IReadOnlyList<Player> Players => _players;
		public int CurrentRound { get; private set; }
		public int CurrentTurn { get; private set; }
		public int CurrentTurnTaker { get; private set; }
		public int PlayerWhoStartedTurn { get; private set; }
		public CardStack SandDeck { get; }
		public CardStack BloodDeck { get; }
		public CardStack SandDiscardPile { get; }
		public CardStack BloodDiscardPile { get; }
		public bool IsRoundOver { get; private set; }
		public bool IsTurnOver { get; private set; }
		public RoundResultList RoundResults { get; private set; }

		public event Action<int> OnTurnStart;

		public KesselSabaccGameModel()
		{
			_players = new List<Player>();
			CurrentRound = 0;
			CurrentTurn = 1;
			PlayerWhoStartedTurn = 0;
			CurrentTurnTaker = 0;
			IsRoundOver = false;
			IsTurnOver = false;
			SandDeck = new CardStack( "Sand Deck", true );
			BloodDeck = new CardStack( "Blood Deck", true );
			SandDiscardPile = new CardStack( "Sand Discard Pile", false );
			BloodDiscardPile = new CardStack( "Blood Discard Pile", false );
			RoundResults = new RoundResultList();
		}

		public void AddPlayer(Player player)
		{
			_players.Add( player );
		}

		public void AdvanceRound()
		{
			CurrentRound++;
			CurrentTurn = 1;
			CurrentTurnTaker = 0;
			IsRoundOver = false;
			IsTurnOver = false;
			OnTurnStart?.Invoke( CurrentTurn );
		}

		public void AdvanceTurn()
		{
			if ( CurrentTurn < TURNS_PER_ROUND )
			{
				CurrentTurn++;
				CurrentTurnTaker = 0;
				IsTurnOver = false;
				foreach ( var p in _players )
				{
					p.HasStoodThisTurn = false;
					p.DrewCardThisTurn = false;
				}
				OnTurnStart?.Invoke( CurrentTurn );
			}
			else
			{
				IsRoundOver = true;
			}
		}

		public void AdvanceTurnTaker()
		{
			if ( CurrentTurnTaker < _players.Count - 1 )
			{
				CurrentTurnTaker += 1;
			}
			else
			{
				IsTurnOver = true;
			}
		}

		public bool IsGameOver()
		{
			int remainingPlayerCount = 0;
			foreach ( Player player in _players )
			{
				if ( !player.IsDisqualified ) remainingPlayerCount++;
			}
			return remainingPlayerCount == 1;
		}
	}
}
