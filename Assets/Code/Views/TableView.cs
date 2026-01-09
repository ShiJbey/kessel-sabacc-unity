using UnityEngine;

namespace KesselSabacc.Views
{
	public class TableView : MonoBehaviour
	{
		[SerializeField]
		private CardStackView _sandDiscardPileView;
		[SerializeField]
		private CardStackView _sandDeckView;
		[SerializeField]
		private CardStackView _bloodDeckView;
		[SerializeField]
		private CardStackView _bloodDiscardPileView;

		public CardStackView SandDiscardPileView => _sandDiscardPileView;
		public CardStackView SandDeckView => _sandDeckView;
		public CardStackView BloodDeckView => _bloodDeckView;
		public CardStackView BloodDiscardPileView => _bloodDiscardPileView;

		public void Initialize(Model.KesselSabacc game)
		{
			_sandDiscardPileView.Initialize( game.SandDiscardPile );
			_sandDeckView.Initialize( game.SandDeck );
			_bloodDeckView.Initialize( game.BloodDeck );
			_bloodDiscardPileView.Initialize( game.BloodDiscardPile );
		}
	}
}
