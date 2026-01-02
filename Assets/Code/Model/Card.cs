using System;

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

		public event Action<int> OnValueChanged;

		public Card(CardSuit suit, CardType cardType)
		{
			_suit = suit;
			_cardType = cardType;
			_value = (int)cardType;
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
