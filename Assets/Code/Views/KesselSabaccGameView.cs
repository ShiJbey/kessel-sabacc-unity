using KesselSabacc.Gameplay;
using KesselSabacc.Model;
using KesselSabacc.UI;
using UnityEngine;

namespace KesselSabacc.Views
{
	public class KesselSabaccGameView : MonoBehaviour
	{
		[Header( "UI References" )]
		public GameHUD hud;
		public DrawCardUI drawCardUI;
		public ShiftTokenTargetSelectionUI shiftTokenTargetSelectionUI;
		public DiscardCardUI discardCardUI;
		public RoundNotificationUI roundNotificationUI;
		public ShiftTokenNotificationUI shiftTokenNotificationUI;
		public DisqualifiedNotificationUI disqualifiedNotificationUI;
		public GameOverNotificationUI gameOverNotificationUI;
		public DiceRollUI diceRollUI;
		public RoundEndUI roundEndUI;

		[Header( "View References" )]
		public TableView tableView;

		private void Start()
		{
			drawCardUI.Hide();
			shiftTokenTargetSelectionUI.Hide();
			discardCardUI.Hide();
			roundNotificationUI.Hide();
			shiftTokenNotificationUI.Hide();
			disqualifiedNotificationUI.Hide();
			gameOverNotificationUI.Hide();
			diceRollUI.Hide();
			roundEndUI.Hide();
		}

		public void Initialize(KesselSabaccGameController gameController)
		{
			hud.Initialize( gameController.Model, this, 0 );
			tableView.Initialize( gameController );
			roundEndUI.Initialize( gameController.Model );
			gameOverNotificationUI.Initialize( gameController );
		}
	}
}
