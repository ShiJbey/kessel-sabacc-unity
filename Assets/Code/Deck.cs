using System.Collections.Generic;


namespace Sabacc
{
    public class Deck
    {
		private CardSuit m_Suit;

		private List<Card> m_Cards;

		public CardSuit Suit => m_Suit;

		public List<Card> Cards => m_Cards;

		public bool IsEmpty => m_Cards.Count > 0;

		public Deck(CardSuit suit, List<Card> cards)
		{
			m_Suit = suit;
			m_Cards = cards;
		}

		public Card DrawCard()
		{
			Card topCard = m_Cards[m_Cards.Count - 1];
			m_Cards.RemoveAt( m_Cards.Count - 1 );
			return topCard;
		}
    }
}
