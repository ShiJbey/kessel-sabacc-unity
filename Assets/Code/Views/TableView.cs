using KesselSabacc.Gameplay;
using KesselSabacc.Model;
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
			_sandDiscardPileView.Initialize( gameController, gameController.Model.SandDiscardPile );
			_sandDeckView.Initialize( gameController, gameController.Model.SandDeck );
			_bloodDeckView.Initialize( gameController, gameController.Model.BloodDeck );
			_bloodDiscardPileView.Initialize( gameController, gameController.Model.BloodDiscardPile );
		}
	}
}
