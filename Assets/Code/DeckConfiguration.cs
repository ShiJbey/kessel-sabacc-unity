using System;
using System.Collections.Generic;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc
{
	[CreateAssetMenu( fileName = "DeckConfiguration", menuName = "Sabacc/DeckConfiguration" )]
	public class DeckConfiguration : ScriptableObject
	{
		public Sprite bloodCardBack;
		public Sprite sandCardBack;
		public DeckCardConfig[] cards;

		public Dictionary<CardType, int> GetCardCountsByType()
		{
			Dictionary<CardType, int> cardCountsByType = new();
			foreach (DeckCardConfig cardConfig in cards)
			{
				cardCountsByType[cardConfig.cardType] = cardConfig.count;
			}
			return cardCountsByType;
		}

		public Sprite GetFrontSprite(CardSuit cardSuit, CardType cardType)
		{
			foreach (DeckCardConfig cardConfig in cards)
			{
				if (cardConfig.cardType == cardType)
				{
					switch (cardSuit)
					{
						case CardSuit.BLOOD:
							return cardConfig.bloodFront;
						case CardSuit.SAND:
							return cardConfig.sandFront;
						default:
							return null;
					}
				}
			}
			return null;
		}

		public Sprite GetBackSprite(CardSuit cardSuit)
		{
			switch (cardSuit)
			{
				case CardSuit.BLOOD:
					return bloodCardBack;
				case CardSuit.SAND:
					return sandCardBack;
				default:
					return null;
			}
		}
	}

	[Serializable]
	public class DeckCardConfig
	{
		public Sprite bloodFront;
		public Sprite sandFront;
		public CardType cardType;
		public int count;
	}
}
