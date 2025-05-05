using System.Collections.Generic;
using UnityEngine;

namespace Sabacc
{
	/// <summary>
	/// Visualizes a deck of Kessel Sabacc Cards
	/// </summary>
	public class DeckView : MonoBehaviour
	{
		[SerializeField]
		private DeckConfiguration m_DeckConfig;

		private Stack<CardData> m_Cards = new Stack<CardData>();

		private void Start()
		{
			ResetDeck();
		}

		public Card DrawCard()
		{
			if ( m_Cards.Count == 0 ) return null;

			CardData cardData = m_Cards.Pop();

			return new Card( cardData );
		}

		/// <summary>
		/// Clears all cards from the deck, generates new ones, and shuffles.
		/// </summary>
		public void ResetDeck()
		{
			List<CardData> cards = new List<CardData>();

			foreach (DeckCardCount cardCount in m_DeckConfig.CardCounts)
			{
				for (int i = 0; i < cardCount.count; i++)
				{
					cards.Add( cardCount.cardData );
				}
			}

			ShuffleDeck( cards );
			m_Cards = new Stack<CardData>( cards );
		}

		private static void ShuffleDeck(List<CardData> list)
		{
			System.Random random = new System.Random();
			int n = list.Count;
			for ( int i = 0; i < n; i++ )
			{
				int r = i + random.Next( n - i );
				CardData temp = list[r];
				list[r] = list[i];
				list[i] = temp;
			}
		}
	}
}
