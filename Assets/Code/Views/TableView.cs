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

		public void Initialize(KesselSabaccGameController gameController, int playerIndex)
		{
			_sandDiscardPileView.Initialize( gameController.Model.SandDiscardPile, CardZone.Discard );
			_sandDeckView.Initialize( gameController.Model.SandDeck, CardZone.Deck );
			_bloodDeckView.Initialize( gameController.Model.BloodDeck, CardZone.Deck );
			_bloodDiscardPileView.Initialize( gameController.Model.BloodDiscardPile, CardZone.Discard );

			for ( int i = 0; i < gameController.Players.Count; i++ )
			{
				if ( i == playerIndex ) continue;

				playerHands[i].Initialize( gameController.Players[i], gameController.playerColors[i] );
			}
		}
	}
}
