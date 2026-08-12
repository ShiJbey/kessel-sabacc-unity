using System;

namespace KesselSabacc.Model
{
	public class StandardHandScoreStrategy : IHandScoreStrategy
	{
		public int ScoreHand(Hand hand, CardType primeSabaccType = CardType.SYLOP)
		{
			int score = 0;

			if (hand.HasPrimeSabacc(primeSabaccType))
				score += 1000;

			if (hand.HasSylopSabacc())
				score += 500;

			if (hand.HasSabacc())
				score += 250;

			score += GetCardDifferenceScore(hand);

			score += GetCardTotalValueScore(hand);

			return score;
		}

		public int GetCardDifferenceScore(Hand hand)
		{
			int score = 10;

			if (!hand.HasSabacc())
			{
				score -= Math.Abs(hand.Cards[0].Value - hand.Cards[1].Value);
			}

			return score * 10;
		}

		public int GetCardTotalValueScore(Hand hand)
		{
			int score = 20;

			score -= hand.Cards[0].Value + hand.Cards[1].Value;

			return score;
		}
	}
}
