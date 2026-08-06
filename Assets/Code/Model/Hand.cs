using System;
using System.Collections.Generic;

namespace KesselSabacc.Model
{
	public class Hand
	{
		private List<Card> _cards;

		public IReadOnlyList<Card> Cards => _cards;

		public Hand() : this(new List<Card>())
		{

		}

		public Hand(List<Card> cards)
		{
			_cards = cards;
		}

		public Hand(Hand other)
		{
			_cards = new List<Card>(other._cards);
		}

		/// <summary>
		/// Remove all cards from the hard.
		/// </summary>
		public void Clear()
		{
			_cards.Clear();
		}

		/// <summary>
		/// Create a copy of the hand with the given card added.
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public Hand CopyWithCard(Card card)
		{
			Hand updatedHand = new Hand(this);
			updatedHand.Add(card);
			return updatedHand;
		}

		/// <summary>
		/// Create a copy of the hand with the card at the given index removed.
		/// </summary>
		/// <param name="cardIndex"></param>
		/// <returns></returns>
		public Hand CopyWithCardRemoved(int cardIndex)
		{
			Hand updatedHand = new Hand(this);
			updatedHand._cards.RemoveAt(cardIndex);
			return updatedHand;
		}

		/// <summary>
		/// Add the given card to the hand.
		/// </summary>
		/// <param name="card"></param>
		public void Add(Card card)
		{
			_cards.Add( card );
		}

		/// <summary>
		/// Remove the given card from the list if it exists.
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public bool Remove(Card card)
		{
			return _cards.Remove( card );
		}

		/// <summary>
		/// Remove the card at the given index.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			_cards.RemoveAt(index);
		}

		/// <summary>
		/// Get all cards of a given suit, or an empty collection if none found.
		/// </summary>
		/// <param name="suit"></param>
		/// <returns></returns>
		public Card[] GetCardsOfSuit(CardSuit suit)
		{
			List<Card> cards = new();

			foreach ( var card in _cards )
			{
				if ( card.Suit == suit )
				{
					cards.Add( card );
				}
			}

			return cards.ToArray();
		}

		/// <summary>
		/// Get the first card of a given suit or null if none found.
		/// </summary>
		/// <param name="suit"></param>
		/// <returns></returns>
		public Card GetFirstCardOfSuit(CardSuit suit)
		{
			foreach ( var card in _cards )
			{
				if ( card.Suit == suit )
				{
					return card;
				}
			}
			return null;
		}

		/// <summary>
		/// If the player contains multiples of any suit. Return them as valid options
		/// of cards to discard.
		/// </summary>
		/// <returns></returns>
		public int[] GetDiscardOptions()
		{
			Dictionary<CardSuit, int> suitCounts = new();
			List<int> discardOptions = new();

			for (int i = 0; i < _cards.Count; i++)
			{
				Card card = _cards[i];

				if (!suitCounts.ContainsKey(card.Suit))
				{
					suitCounts[card.Suit] = 0;
				}

				suitCounts[card.Suit]++;

				if (suitCounts[card.Suit] > 1)
				{
					discardOptions.Add(i);
				}
			}

			return discardOptions.ToArray();
		}

		/// <summary>
		/// Return the difference between the first two cards in the player's hand.
		/// </summary>
		/// <returns></returns>
		public int GetCardDifference()
		{
			return Math.Abs(Cards[0].Value - Cards[1].Value);
		}

		/// <summary>
		/// Return true if the hand contains doubles or at least one sylop card.
		/// </summary>
		/// <returns></returns>
		public bool HasSabacc()
		{
			if (Cards.Count != 2) return false;

			return HasSylopSabacc() || Cards[0].Value == Cards[1].Value;
		}

		/// <summary>
		/// Return true if the hand contains at least one sylop card.
		/// </summary>
		/// <returns></returns>
		public bool HasSylopSabacc()
		{
			if (Cards.Count != 2) return false;

			return Cards[0].CardType == CardType.SYLOP || Cards[1].CardType == CardType.SYLOP;
		}

		/// <summary>
		/// Return true if the hand contains two sylop cards.
		/// </summary>
		/// <returns></returns>
		public bool HasPrimeSabacc(CardType primeSabaccType = CardType.SYLOP)
		{
			if (Cards.Count != 2) return false;

			return Cards[0].CardType == primeSabaccType && Cards[1].CardType == primeSabaccType;
		}
	}
}
