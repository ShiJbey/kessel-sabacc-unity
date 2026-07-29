using KesselSabacc.Gameplay;
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

		public HandView[] playerHands;

		public CardStackView SandDiscardPileView => _sandDiscardPileView;
		public CardStackView SandDeckView => _sandDeckView;
		public CardStackView BloodDeckView => _bloodDeckView;
		public CardStackView BloodDiscardPileView => _bloodDiscardPileView;

		public void Initialize(KesselSabaccGameController gameController)
		{
			_sandDiscardPileView.Initialize( gameController.Model.SandDiscardPile, CardZone.Discard );
			_sandDeckView.Initialize( gameController.Model.SandDeck, CardZone.Deck );
			_bloodDeckView.Initialize( gameController.Model.BloodDeck, CardZone.Deck );
			_bloodDiscardPileView.Initialize( gameController.Model.BloodDiscardPile, CardZone.Discard );
		}
	}
}
