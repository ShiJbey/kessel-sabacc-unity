using UnityEngine;

namespace Sabacc
{
    public class Card
    {
        private CardData m_Data;

        public string Name => m_Data.Name;

        public CardSuit Suit => m_Data.Suit;

        public CardValue Value => m_Data.Value;

		public int ScoreOverride { get; set;  }

        public Sprite FrontSprite => m_Data.FrontSprite;

		public Sprite BackSprite => m_Data.BackSprite;

        public Card(CardData data)
        {
            m_Data = data;
			ScoreOverride = -1;
        }
    }
}
