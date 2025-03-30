using System.Collections.Generic;
using UnityEngine;


namespace Sabacc
{
	public class Player
	{


		private int m_Chips;

		private int m_ChipsInvestedThisTurn;

		private List<Card> m_Cards;

		private Card m_DrawnBloodCard;
		private Card m_BloodCard;
		private Card m_SandCard;
		private Card m_DawnSandCard;

		private List<ShiftToken> m_ShiftTokens;

		public int Chips
		{
			get => m_Chips;
			set => m_Chips = value;
		}

		public Card DrawnBloodCard { get; set; }
		public Card BloodCard { get; set; }
		public Card SandCard { get; set; }
		public Card DawnSandCard { get; set; }

		public List<Card> Cards => m_Cards;

		/// <summary>
		/// Add the given card to the player's hand.
		/// </summary>
		/// <param name="card"></param>
		public void AddCardToHand(Card card)
		{
			m_Cards.Add( card );
		}


		/// <summary>
		/// Discard the card at the given index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Card DiscardCard(int index)
		{
			Card card = m_Cards[index];
			m_Cards.RemoveAt( index );
			return card;
		}

		/// <summary>
		/// Play the shift token at the given index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public ShiftToken PlayShiftToken(int index)
		{
			if ( m_ShiftTokens[index].IsUsed )
			{
				Debug.LogError( "Shift token has already been used." );
				return null;
			}

			m_ShiftTokens[index].IsUsed = true;
			return m_ShiftTokens[index];
		}
	}
}

