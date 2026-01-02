using System;

namespace KesselSabacc.Model
{
	public class Player
	{
		private string _name;
		private int _chips;
		private int _chipsInvested;
		private Hand _hand;
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

		public Hand Hand
		{
			get => _hand;
		}

		public bool HasStoodThisTurn
		{
			get => _hasStoodThisTurn;
		}

		public bool IsDisqualified => _isDisqualified;

		public event Action<string> OnNameChanged;
		public event Action<int> OnChipsChanged;
		public event Action<int> OnChipsInvestedChanged;
		public event Action<Hand> OnHandChanged;
		public event Action OnDisqualified;


		public Player(string name = "")
		{
			_name = name;
			_chips = 0;
			_chipsInvested = 0;
			_hand = new Hand();
			_hasStoodThisTurn = false;
			_isDisqualified = false;
		}

		public void ResetForNewRound()
		{
			_chipsInvested = 0;
			_hand = new Hand();
			_hasStoodThisTurn = false;
		}

		public void AddCardToHand(Card card)
		{
			if ( card == null )
			{
				throw new NullReferenceException( "Card cannot be null" );
			}

			if ( card.Suit == CardSuit.BLOOD )
			{
				if ( _hand.bloodCard == null )
				{
					_hand.bloodCard = card;
				}
				else
				{
					_hand.drawnCard = card;
				}
			}
			else
			{
				if ( _hand.sandCard == null )
				{
					_hand.sandCard = card;
				}
				else
				{
					_hand.drawnCard = card;
				}
			}
			OnHandChanged?.Invoke( _hand );
		}

		public void DiscardCardFromHand(Card card)
		{
			if ( card == null )
			{
				throw new NullReferenceException( "Card cannot be null" );
			}

			if ( _hand.bloodCard == card )
			{
				_hand.bloodCard = null;
				OnHandChanged?.Invoke( _hand );
			}
			else if ( _hand.sandCard == card )
			{
				_hand.sandCard = null;
				OnHandChanged?.Invoke( _hand );
			}
			else if ( _hand.drawnCard == card )
			{
				_hand.drawnCard = null;
				OnHandChanged?.Invoke( _hand );
			}
		}

		public void DisqualifyPlayer()
		{
			_isDisqualified = true;
			OnDisqualified?.Invoke();
		}
	}
}
