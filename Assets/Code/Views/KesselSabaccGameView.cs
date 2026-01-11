using KesselSabacc.Model;
using KesselSabacc.UI;
using UnityEngine;

namespace KesselSabacc.Views
{
	public class KesselSabaccGameView : MonoBehaviour
	{
		[Header( "References" )]
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

		public void Initialize(KesselSabaccGameModel model)
		{
			hud.Initialize( model, this, 0 );
			tableView.Initialize( model );
		}
	}
}
