using System;
using System.Collections.Generic;
using System.Linq;

namespace KesselSabacc.Model
{
	public class Player
	{
		private string _name;
		private int _chips;
		private int _chipsInvested;
		private List<Card> _hand;
		private bool _hasStoodThisTurn;
		private bool _isDisqualified;

		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				OnNameChanged?.Invoke( _name );
			}
		}

		public int Chips
		{
			get => _chips;
			set
			{
				_chips = value;
				OnChipsChanged?.Invoke( _chips );
			}
		}

		public int ChipsInvested
		{
			get => _chipsInvested;
			set
			{
				_chipsInvested = value;
				OnChipsInvestedChanged?.Invoke( _chipsInvested );
			}
		}

		public IReadOnlyList<Card> Hand => _hand;
		public bool HasStoodThisTurn => _hasStoodThisTurn;
		public bool IsDisqualified => _isDisqualified;

		public event Action<string> OnNameChanged;
		public event Action<int> OnChipsChanged;
		public event Action<int> OnChipsInvestedChanged;
		public event Action OnDisqualified;

		public Player(string name = "")
		{
			_name = name;
			_chips = 0;
			_chipsInvested = 0;
			_hand = new List<Card>();
			_hasStoodThisTurn = false;
			_isDisqualified = false;
		}

		public void ResetForNewRound()
		{
			_chipsInvested = 0;
			_hand.Clear();
			_hasStoodThisTurn = false;
		}

		public void AddCardToHand(Card card)
		{
			if ( card == null )
			{
				throw new NullReferenceException( "Card cannot be null" );
			}
			_hand.Add( card );
		}

		public bool DiscardCardFromHand(Card card)
		{
			return _hand.Remove( card );
		}

		public Card[] GetCardsOfSuit(CardSuit suit)
		{
			List<Card> cards = new();

			foreach ( var card in _hand )
			{
				if ( card.Suit == suit )
				{
					cards.Add( card );
				}
			}

			return cards.ToArray();
		}

		public Card GetFirstCardOfSuit(CardSuit suit)
		{
			foreach ( var card in _hand )
			{
				if ( card.Suit == suit )
				{
					return card;
				}
			}
			return null;
		}

		public void DisqualifyPlayer()
		{
			_isDisqualified = true;
			OnDisqualified?.Invoke();
		}
	}
}
