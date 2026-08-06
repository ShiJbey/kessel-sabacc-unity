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
		/// Is the player currently disqualified from play.
		/// </summary>
		private bool _isDisqualified;
		private bool _isRolling;

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

		public bool IsRolling
		{
			get => _isRolling;
			set
			{
				_isRolling = value;
				OnIsRollingChanged?.Invoke(value);
			}
		}

		public Hand Hand { get; }
		public bool HasStoodThisTurn { get; set; }
		public bool DrewCardThisTurn { get; set; }
		public bool IsDisqualified => _isDisqualified;

		public event Action<string> OnNameChanged;
		public event Action<int> OnChipsChanged;
		public event Action<int> OnChipsInvestedChanged;
		public event Action OnDisqualified;
		public event Action<bool> OnIsRollingChanged;

		public Player(string name, int startingChips)
		{
			_name = name;
			_chips = startingChips;
			_chipsInvested = 0;
			StartingChips = startingChips;
			Hand = new Hand();
			HasStoodThisTurn = false;
			_isDisqualified = false;
		}

		public void ResetForNewRound()
		{
			_chipsInvested = 0;
			Hand.Clear();
			HasStoodThisTurn = false;
		}

		public void AddCardToHand(Card card)
		{
			if ( card == null )
			{
				throw new NullReferenceException( "Card cannot be null" );
			}
			Hand.Add( card );
		}

		public bool DiscardCardFromHand(Card card)
		{
			return Hand.Remove( card );
		}

		public Card[] GetCardsOfSuit(CardSuit suit)
		{
			List<Card> cards = new();

			foreach ( var card in Hand.Cards )
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
			Hand.Clear();
		}

		public Card GetFirstCardOfSuit(CardSuit suit)
		{
			foreach ( var card in Hand.Cards )
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
