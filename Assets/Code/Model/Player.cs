using System;
using System.Collections.Generic;

namespace KesselSabacc.Model
{
	public class Player
	{
		/// <summary>
		/// Name assigned to this player.
		/// </summary>
		private string _name;
		/// <summary>
		/// The current number of chips available to the player.
		/// </summary>
		private int _chips;
		/// <summary>
		/// The number of chips invested this round.
		/// </summary>
		private int _chipsInvested;
		/// <summary>
		/// Cards currently in the player's hand.
		/// </summary>
		private List<Card> _hand;
		/// <summary>
		/// Is the player currently disqualified from play.
		/// </summary>
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

		public int StartingChips { get; }

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
		public bool HasStoodThisTurn { get; set; }
		public bool DrewCardThisTurn { get; set; }
		public bool IsDisqualified => _isDisqualified;

		public event Action<string> OnNameChanged;
		public event Action<int> OnChipsChanged;
		public event Action<int> OnChipsInvestedChanged;
		public event Action OnDisqualified;

		public Player(string name, int startingChips)
		{
			_name = name;
			_chips = startingChips;
			_chipsInvested = 0;
			StartingChips = startingChips;
			_hand = new List<Card>();
			HasStoodThisTurn = false;
			_isDisqualified = false;
		}

		public void ResetForNewRound()
		{
			_chipsInvested = 0;
			_hand.Clear();
			HasStoodThisTurn = false;
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

		public void ClearHand()
		{
			_hand.Clear();
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
