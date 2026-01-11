using System;

namespace KesselSabacc.Model
{
	/// <summary>
	/// Data representation of a single card in the game.
	/// </summary>
	public class Card
	{
		public CardSuit Suit { get; }
		public CardType CardType { get; }
		public int Value { get; private set; }

		public event Action<int> OnValueChanged;

		public Card(CardSuit suit, CardType cardType)
		{
			Suit = suit;
			CardType = cardType;
			Value = (int)cardType;
		}

		public void SetValue(int value)
		{
			Value = value;
			OnValueChanged?.Invoke( Value );
		}

		public void ResetValue()
		{
			Value = (int)CardType;
		}

		public bool IsValueModified()
		{
			return Value != (int)CardType;
		}

		public override string ToString()
		{
			return $"Card(suit={Suit}, type={CardType}, value={Value})";
		}
	}
}
