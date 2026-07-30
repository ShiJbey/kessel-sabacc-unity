using System;
using System.Collections.Generic;
using KesselSabacc.Gameplay;
using KesselSabacc.Gameplay.PlayerActions;

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
		public bool IsPlayerTurnOver { get; set; }
		public RoundResultList RoundResults { get; private set; }

		public event Action<int> OnTurnStart;

		public KesselSabaccGameModel()
		{
			_players = new List<Player>();
			CurrentRound = 1;
			CurrentTurn = 1;
			PlayerWhoStartedTurn = 0;
			CurrentTurnTaker = 0;
			IsRoundOver = false;
			IsTurnOver = false;
			SandDeck = new CardStack();
			BloodDeck = new CardStack();
			SandDiscardPile = new CardStack();
			BloodDiscardPile = new CardStack();
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
			CurrentTurnTaker = GetNextEligiblePlayerIndex( PlayerWhoStartedTurn );
			PlayerWhoStartedTurn = CurrentTurnTaker;
			IsRoundOver = false;
			IsTurnOver = false;
			OnTurnStart?.Invoke( CurrentTurn );
		}

		public void AdvanceTurn()
		{
			if ( CurrentTurn < TURNS_PER_ROUND )
			{
				CurrentTurn++;
				CurrentTurnTaker = GetNextEligiblePlayerIndex( PlayerWhoStartedTurn );
				PlayerWhoStartedTurn = CurrentTurnTaker;
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
			CurrentTurnTaker = GetNextEligiblePlayerIndex( CurrentTurnTaker );
			IsPlayerTurnOver = false;
			if ( CurrentTurnTaker == PlayerWhoStartedTurn )
			{
				IsTurnOver = true;
			}
		}

		private int GetNextEligiblePlayerIndex(int start)
		{
			for ( int i = start + 1; i < start + Players.Count; i++ )
			{
				int playerIndex = i % Players.Count;
				if ( !Players[playerIndex].IsDisqualified )
				{
					return playerIndex;
				}
			}

			return start;
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

		public Player GetWinner()
		{
			List<Player> remainingPlayers = new();

			foreach ( Player player in _players )
			{
				if ( !player.IsDisqualified ) remainingPlayers.Add( player );
			}

			if ( remainingPlayers.Count == 1 ) return remainingPlayers[0];

			return null;
		}

		public List<PlayerAction> GetLegalActions(int playerIndex)
		{
			List<PlayerAction> legalActions = new();

			if ( CurrentTurnTaker != playerIndex ) return legalActions;

			Player player = Players[playerIndex];

			// Return an empty list.
			if ( player.HasStoodThisTurn ) return legalActions;

			if ( player.DrewCardThisTurn )
			{
				var sandCards = player.GetCardsOfSuit( CardSuit.SAND );
				if ( sandCards.Length > 1 )
				{
					legalActions.Add( new DiscardCardAction( playerIndex, sandCards[0] ) );
					legalActions.Add( new DiscardCardAction( playerIndex, sandCards[1] ) );
				}

				var bloodCards = player.GetCardsOfSuit( CardSuit.BLOOD );
				if ( bloodCards.Length > 1 )
				{
					legalActions.Add( new DiscardCardAction( playerIndex, bloodCards[0] ) );
					legalActions.Add( new DiscardCardAction( playerIndex, bloodCards[1] ) );
				}
			}
			else
			{
				if ( !SandDiscardPile.IsEmpty() && player.Chips > 0 )
				{
					legalActions.Add(
						new DrawCardAction(
							playerIndex,
							SandDiscardPile.Peek(),
							SandDiscardPile
						)
					);
				}

				if ( !SandDeck.IsEmpty() && player.Chips > 0 )
				{
					legalActions.Add(
						new DrawCardAction(
							playerIndex,
							SandDeck.Peek(),
							SandDeck
						)
					);
				}

				if ( !BloodDiscardPile.IsEmpty() && player.Chips > 0 )
				{
					legalActions.Add(
						new DrawCardAction(
							playerIndex,
							BloodDiscardPile.Peek(),
							BloodDiscardPile
						)
					);
				}

				if ( !BloodDeck.IsEmpty() && player.Chips > 0 )
				{
					legalActions.Add(
						new DrawCardAction(
							playerIndex,
							BloodDeck.Peek(),
							BloodDeck
						)
					);
				}

				legalActions.Add( new StandAction( playerIndex ) );
			}

			return legalActions;
		}
	}
}
