using System.Collections.Generic;
using UnityEngine;


namespace Sabacc
{
	/// <summary>
	/// Kessel Sabacc matches involve 2-4 players and last mutliple rounds.
	/// Each round last three turns, during which players may exchange chips
	/// to try and achieve a better hand.
	/// </summary>
	public class SabaccMatch
	{
		public const int TURNS_PER_ROUND = 3;

		private int m_StartingChips;

		private Player[] m_Players;

		/// <summary>
		/// The current round of the game.
		/// </summary>
		private int m_CurrentRound;

		/// <summary>
		/// The current turn within the round.
		/// </summary>
		private int m_CurrentTurn;

		/// <summary>
		/// The index of the play who is currently taking their turn.
		/// </summary>
		private int m_CurrentTurnTaker;

		/// <summary>
		/// The index of the player who went first this round.
		/// </summary>
		private int m_PlayerWhoStartedRound;

		private Deck m_SandRejectDeck;

		private Deck m_SandDeck;

		private Deck m_BloodDeck;

		private Deck m_BloodRejectDeck;

		public int StartingChips => m_StartingChips;
		public Player[] Players => m_Players;

		public SabaccMatch(int startingChips)
		{
			m_CurrentRound = 1;
			m_CurrentTurnTaker = 0;
			m_PlayerWhoStartedRound = 0;
			m_CurrentTurn = 0;
			m_StartingChips = startingChips;
		}

		public int IncrementTurn()
		{
			return -1;
		}

		public int IncrementRound()
		{
			m_CurrentRound += 1;
			return m_CurrentRound;
		}

		public int IncrementTurnTaker()
		{
			m_CurrentTurnTaker++;
			m_CurrentTurnTaker = m_CurrentTurnTaker % m_Players.Length;
			return m_CurrentTurnTaker;
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
	}
}

