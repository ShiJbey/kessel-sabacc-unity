using System;
using System.Collections.Generic;
using KesselSabacc.Model.PlayerActions;

namespace KesselSabacc.Model
{
	public class KesselSabaccGameModel
	{
		public const int TURNS_PER_ROUND = 3;

		private List<Player> _players;
		private Dictionary<CardType, int> _deckConfig = new();

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
		public IHandScoreStrategy HandScorer { get; set; }
		public CardType PrimeSabaccType { get; set; } = CardType.SYLOP;

		public event Action<int> OnTurnStart;
		public event Action<CardDrawnEventData> OnCardDrawn;
		public event Action<CardDiscardedEventData> OnCardDiscarded;
		public event Action OnDiscardPilesCleared;
		public event Action OnDrawPilesCleared;
		public event Action OnDrawPilesReset;
		public event Action OnHandsCleared;
		public event Action<HandDealtEventData> OnHandDealt;

		public KesselSabaccGameModel()
		{
			_players = new List<Player>();
			CurrentRound = 1;
			CurrentTurn = 1;
			PlayerWhoStartedTurn = 0;
			CurrentTurnTaker = 0;
			IsRoundOver = false;
			IsTurnOver = false;
			SandDeck = new CardStack( CardStack.DeckKind.SAND_DRAW );
			BloodDeck = new CardStack( CardStack.DeckKind.BLOOD_DRAW );
			SandDiscardPile = new CardStack( CardStack.DeckKind.SAND_DISCARD );
			BloodDiscardPile = new CardStack( CardStack.DeckKind.BLOOD_DISCARD );
			RoundResults = new RoundResultList();
			HandScorer = new StandardHandScoreStrategy();
		}

		public void SetDeckConfig(Dictionary<CardType, int> config)
		{
			_deckConfig = new Dictionary<CardType, int>( config );
		}

		public void AddPlayer(Player player)
		{
			_players.Add( player );
		}

		public void AdvanceRound()
		{
			CurrentRound++;
			CurrentTurn = 1;
			PlayerWhoStartedTurn = GetNextEligiblePlayerIndex( PlayerWhoStartedTurn );
			CurrentTurnTaker = PlayerWhoStartedTurn;
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
				// CurrentTurnTaker = GetNextEligiblePlayerIndex( PlayerWhoStartedTurn );
				// PlayerWhoStartedTurn = CurrentTurnTaker;
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

		public int GetPlayerScore(int playerIndex)
		{
			return HandScorer.ScoreHand( _players[playerIndex].Hand, PrimeSabaccType );
		}

		public void DealHands()
		{
			for ( int i = 0; i < _players.Count; i++ )
			{
				var player = _players[i];
				if ( player.IsDisqualified ) continue;

				// Deal sand and blood cards
				_players[i].Hand.Add( SandDeck.Pop() );
				_players[i].Hand.Add( BloodDeck.Pop() );

				OnHandDealt?.Invoke(new HandDealtEventData(i));
			}
		}

		public void ResetDrawPiles()
		{
			foreach ( var entry in _deckConfig )
			{
				CardType cardType = entry.Key;
				int quantity = entry.Value;

				for ( int i = 0; i < quantity; i++ )
				{
					SandDeck.Add( new Card( CardSuit.SAND, cardType ) );
					BloodDeck.Add( new Card( CardSuit.BLOOD, cardType ) );
				}
			}

			SandDeck.Shuffle();
			BloodDeck.Shuffle();

			SandDiscardPile.Add( SandDeck.Pop() );
			BloodDiscardPile.Add( BloodDeck.Pop() );

			OnDrawPilesReset?.Invoke();
		}

		public void ClearDrawPiles()
		{
			SandDeck.Clear();
			BloodDeck.Clear();
			OnDrawPilesCleared?.Invoke();
		}

		public void ClearDiscardPiles()
		{
			SandDiscardPile.Clear();
			BloodDiscardPile.Clear();
			OnDiscardPilesCleared?.Invoke();
		}

		public void ClearHands()
		{
			foreach ( Player player in _players )
			{
				player.ClearHand();
			}
			OnHandsCleared?.Invoke();
		}

		public void DrawCard(int playerIndex, CardStack.DeckKind deck)
		{
			DrawCard( playerIndex, GetDeck( deck ) );
		}

		public CardStack GetDeck(CardStack.DeckKind deck)
		{
			switch ( deck )
			{
				case CardStack.DeckKind.SAND_DISCARD:
					return SandDiscardPile;
				case CardStack.DeckKind.SAND_DRAW:
					return SandDeck;
				case CardStack.DeckKind.BLOOD_DRAW:
					return BloodDeck;
				default:
					return BloodDiscardPile;
			}
		}

		public void DrawCard(int playerIndex, CardStack cardStack)
		{
			if ( cardStack.Cards.Count > 0 )
			{
				Card card = cardStack.Pop();
				_players[playerIndex].Hand.Add( card );
				OnCardDrawn?.Invoke( new CardDrawnEventData( playerIndex, cardStack.Kind, card ) );
			}
		}

		public void DiscardCard(int playerIndex, int cardIndex)
		{
			Card card = _players[playerIndex].Hand.Cards[cardIndex];
			_players[playerIndex].Hand.RemoveAt( cardIndex );
			CardStack discardPile = (card.Suit == CardSuit.SAND) ? SandDiscardPile : BloodDiscardPile;
			discardPile.Add(card);
			OnCardDiscarded?.Invoke( new CardDiscardedEventData( playerIndex, cardIndex, card.Suit ) );
		}

		public void Stand(int playerIndex)
		{
			Player player = _players[playerIndex];
			player.HasStoodThisTurn = true;
		}

		public void ApplyRoundEndResults()
		{
			var bestResult = RoundResults.Results[0];

			foreach ( PlayerRoundResult roundResult in RoundResults.Results )
			{
				roundResult.WonRound = roundResult == bestResult
					|| roundResult.CompareTo( bestResult ) == 0;

				if ( roundResult.WonRound )
				{
					// Winner is not taxed.
					roundResult.Player.Chips = Math.Max(
						0,
						roundResult.Player.Chips
						+ roundResult.Player.ChipsInvested
					);
				}
				else if ( roundResult.Player.Hand.HasSabacc() )
				{
					// Players that lose, but have sabacc are taxed one chip.
					roundResult.Player.Chips = Math.Max(
						0,
						roundResult.Player.Chips
						+ (roundResult.Player.ChipsInvested - 1)
					);
				}
				else
				{
					// Losers without sabacc are taxed the difference of their cards.
					roundResult.Player.Chips = Math.Max(
						0,
						roundResult.Player.Chips
						+ (roundResult.Player.ChipsInvested - roundResult.Player.Hand.GetCardDifference())
					);
				}

				roundResult.Player.ChipsInvested = 0;

				if ( roundResult.Player.Chips == 0 )
				{
					roundResult.Player.DisqualifyPlayer();
				}
			}
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
				var discardOptions = player.Hand.GetDiscardOptions();
				foreach ( int cardIndex in discardOptions )
				{
					Card card = player.Hand.Cards[cardIndex];
					legalActions.Add( new DiscardCardAction( playerIndex, cardIndex, card.CardType, card.Suit ) );
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

				legalActions.Add( new StandAction( playerIndex ) );
			}

			return legalActions;
		}
	}
}
