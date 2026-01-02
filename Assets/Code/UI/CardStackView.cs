using KesselSabacc.Model;

namespace KesselSabacc.UI
{
	/// <summary>
	/// Visual representation of draw decks and discard piles.
	/// </summary>
	public class CardStackView
	{
		public void Initialize(CardStack model)
		{
			model.OnCardAdded += CardStack_OnCardAdded;
			model.OnCardRemoved += CardStack_OnCardRemoved;
			model.OnCardsCleared += CardStack_OnCardsCleared;
		}

		private void CardStack_OnCardAdded(Card card)
		{
			// TODO: Animate card moving to this stack
		}

		private void CardStack_OnCardRemoved(Card card)
		{
			// TODO: Animate care leaving the stack
		}

		private void CardStack_OnCardsCleared()
		{
			// TODO: Play animation of the cards disappearing
		}
	}
}
