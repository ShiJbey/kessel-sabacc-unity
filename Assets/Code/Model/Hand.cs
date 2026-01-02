using System;

namespace KesselSabacc.Model
{
	/// <summary>
	/// The cards in a player's hand.
	/// </summary>
	public class Hand
	{
		public Card sandCard;
		public Card bloodCard;
		public Card drawnCard;

		public bool IsSabaccHand()
		{
			if ( sandCard != null && bloodCard != null )
			{
				// Do not consider this hand a sabacc hand if it has imposter cards
				// without set values.
				if (
					(sandCard.CardType == CardType.IMPOSTER && !sandCard.IsValueModified())
					|| (bloodCard.CardType == CardType.IMPOSTER && !bloodCard.IsValueModified())
				)
				{
					return false;
				}

				// The card values must match.
				return sandCard.Value == bloodCard.Value;
			}
			return false;
		}

		public bool IsPrimeSabaccHand()
		{
			if ( sandCard != null && bloodCard != null )
			{
				// This is only possible with two sylop cards.
				return sandCard.CardType == CardType.SYLOP && bloodCard.CardType == CardType.SYLOP;
			}
			return false;
		}

		/// <summary>
		/// Return the score of this hand that is used to determine the winner.
		/// This method accounts for card values to ensure sabacc hands
		/// are properly ranked.
		/// </summary>
		/// <returns></returns>
		public int GetPerformanceScore()
		{
			int score = 0;

			if ( IsSabaccHand() ) score += 100;

			if ( IsPrimeSabaccHand() ) score += 100;

			return score;
		}

		/// <summary>
		/// Return the difference in value between the two cards.
		/// </summary>
		/// <returns></returns>
		public int GetDifferenceScore()
		{
			if ( sandCard != null && bloodCard != null )
			{
				if ( bloodCard.CardType == CardType.SYLOP && !bloodCard.IsValueModified() )
				{
					return 0;
				}

				if ( sandCard.CardType == CardType.SYLOP && !sandCard.IsValueModified() )
				{
					return 0;
				}

				// This is only possible with two sylop cards.
				return Math.Abs( bloodCard.Value - sandCard.Value );
			}
			return 999_999;
		}

		public override string ToString()
		{
			return $"Hand(sandCard={sandCard}, bloodCard={bloodCard}, drawnCard={drawnCard})";
		}
	}
}
