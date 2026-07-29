using System;
using System.Collections.Generic;

namespace KesselSabacc.Model
{
	/// <summary>
	/// A collection of cards that is treated like a stack data structure.
	/// </summary>
	public class CardStack
	{
		private List<Card> _cards;

		public event Action<Card> OnCardAdded;
		public event Action<Card> OnCardRemoved;
		public event Action OnCardsCleared;

		public IReadOnlyList<Card> Cards => _cards;

		public CardStack()
		{
			_cards = new List<Card>();
		}

		public bool IsEmpty()
		{
			return _cards.Count == 0;
		}

		public Card Peek()
		{
			if ( _cards.Count > 0 )
			{
				return _cards[_cards.Count - 1];
			}
			return null;
		}

		public void Add(Card card)
		{
			_cards.Add( card );
			OnCardAdded?.Invoke( card );
		}

		public Card Pop()
		{
			if ( _cards.Count > 0 )
			{
				Card card = _cards[_cards.Count - 1];
				_cards.RemoveAt( _cards.Count - 1 );
				OnCardRemoved?.Invoke( card );
				return card;
			}
			return null;
		}

		public void Clear()
		{
			_cards.Clear();
			OnCardsCleared?.Invoke();
		}

		public void Shuffle()
		{
			System.Random random = new System.Random();
			int n = _cards.Count;
			for ( int i = 0; i < n; i++ )
			{
				int r = i + random.Next( n - i );
				Card temp = _cards[r];
				_cards[r] = _cards[i];
				_cards[i] = temp;
			}
		}
	}
}
