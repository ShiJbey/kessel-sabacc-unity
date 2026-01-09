using System;
using UnityEngine;

namespace KesselSabacc.Model
{
	/// <summary>
	/// Data representation of a single card in the game.
	/// </summary>
	public class Card
	{
		private CardSuit _suit;
		private CardType _cardType;
		private int _value;
		private Sprite _frontSprite;
		private Sprite _backSprite;
		private bool _isFaceUp;

		public CardSuit Suit => _suit;
		public CardType CardType => _cardType;
		public int Value
		{
			get => _value;
			set
			{
				_value = value;
				OnValueChanged?.Invoke( _value );
			}
		}
		public Sprite FrontSprite => _frontSprite;
		public Sprite BackSprite => _backSprite;
		public bool IsFaceUp
		{
			get => _isFaceUp;
			set
			{
				if ( _isFaceUp != value )
				{
					_isFaceUp = value;
					OnCardFlipped?.Invoke();
				}
			}
		}

		public event Action<int> OnValueChanged;
		public event Action OnCardFlipped;

		public Card(CardSuit suit, CardType cardType, Sprite cardFront, Sprite cardBack)
		{
			_suit = suit;
			_cardType = cardType;
			_value = (int)cardType;
			_frontSprite = cardFront;
			_backSprite = cardBack;
			_isFaceUp = false;
		}

		public void ResetValue()
		{
			Value = (int)CardType;
		}

		public bool IsValueModified()
		{
			return _value != (int)_cardType;
		}

		public override string ToString()
		{
			return $"Card(suit={_suit}, type={_cardType}, value={_value})";
		}
	}
}
