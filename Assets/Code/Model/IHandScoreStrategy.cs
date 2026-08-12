namespace KesselSabacc.Model
{
	public interface IHandScoreStrategy
	{
		/// <summary>
		/// Provide a score for the hand where higher scores are associated with
		/// stronger hands.
		/// </summary>
		/// <param name="hand"></param>
		/// <param name="primeSabaccType"></param>
		/// <returns></returns>
		public int ScoreHand(Hand hand, CardType primeSabaccType = CardType.SYLOP);
	}
}
