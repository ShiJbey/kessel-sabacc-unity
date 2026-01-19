using System;
using KesselSabacc.Model;

namespace KesselSabacc.Gameplay
{
	public static class HandScoreUtils
	{
		public static bool HasSabaccHand(Player player)
		{
			var sandCard = player.GetFirstCardOfSuit( CardSuit.SAND );
			var bloodCard = player.GetFirstCardOfSuit( CardSuit.BLOOD );

			if ( sandCard != null && bloodCard != null )
			{
				if ( sandCard.CardType == CardType.SYLOP || bloodCard.CardType == CardType.SYLOP )
				{
					return true;
				}

				return sandCard.Value == bloodCard.Value;
			}
			return false;
		}

		public static bool HasPrimeSabaccHand(Player player)
		{
			var sandCard = player.GetFirstCardOfSuit( CardSuit.SAND );
			var bloodCard = player.GetFirstCardOfSuit( CardSuit.BLOOD );

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
		public static int GetPerformanceScore(Player player)
		{
			int score = 0;

			if ( HasSabaccHand( player ) ) score += 100;

			if ( HasPrimeSabaccHand( player ) ) score += 100;

			return score;
		}

		/// <summary>
		/// Return the difference in value between the two cards.
		/// </summary>
		/// <returns></returns>
		public static int GetCardDifference(Player player)
		{
			var sandCard = player.GetFirstCardOfSuit( CardSuit.SAND );
			var bloodCard = player.GetFirstCardOfSuit( CardSuit.BLOOD );

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

	}
}
