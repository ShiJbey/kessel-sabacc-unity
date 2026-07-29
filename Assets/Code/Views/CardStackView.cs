using System.Collections;
using System.Collections.Generic;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Views
{
	/// <summary>
	/// Visual representation of draw decks and discard piles.
	/// </summary>
	public class CardStackView : MonoBehaviour
	{
		public CardStack Model { get; private set; }
		private CardZone _cardZone;
		private List<CardView> _cards = new();

		public void Initialize(CardStack stack, CardZone cardZone)
		{
			_cardZone = cardZone;
			Model = stack;
		}

		public int Count()
		{
			return _cards.Count;
		}

		public void Clear()
		{
			foreach ( CardView cardView in _cards )
			{
				Destroy( cardView.gameObject );
			}
			_cards.Clear();
		}

		public CardView Peek()
		{
			if ( _cards.Count > 0 )
			{
				CardView cardView = _cards[_cards.Count - 1];
				return cardView;
			}
			return null;
		}

		public CardView Pop()
		{
			if ( _cards.Count > 0 )
			{
				CardView cardView = _cards[_cards.Count - 1];
				_cards.RemoveAt( _cards.Count - 1 );
				return cardView;
			}
			return null;
		}

		public void AddCard(CardView cardView)
		{
			CardSortingSystem.Instance.AddCardToZone( cardView, _cardZone );

			CardSortingSystem.Instance.AddCardToZone( cardView, CardZone.Discard );

			int numCards = _cards.Count;

			// Slight offset for stacking effect
			Vector3 positionOffset = new Vector3( 0, 0.01f * numCards, -0.01f * numCards );
			cardView.transform.position = transform.position + positionOffset;

			Model.Add( cardView.Card );

			_cards.Add( cardView );
		}
	}
}
