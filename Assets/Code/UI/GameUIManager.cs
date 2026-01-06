using KesselSabacc.Gameplay;
using KesselSabacc.UI.Components;
using UnityEngine;

namespace KesselSabacc.UI
{
	/// <summary>
	/// Central manager for all UI used while playing a game of Kessel Sabacc.
	/// </summary>
	public class GameUIManager : MonoBehaviour
	{
		[SerializeField]
		private GameHUD _hud;
		[SerializeField]
		private DrawCardUI _drawCardUI;
		[SerializeField]
		private ShiftTokenTargetSelectionUI _shiftTokenTargetSelectionUI;
		[SerializeField]
		private DiscardCardUI _discardCardUI;
		[SerializeField]
		private RoundNotificationUI _roundNotificationUI;
		[SerializeField]
		private ShiftTokenNotificationUI _shiftTokenNotificationUI;
		[SerializeField]
		private DisqualifiedNotificationUI _disqualifiedNotificationUI;
		[SerializeField]
		private GameOverNotificationUI _gameOverNotificationUI;
		[SerializeField]
		private DiceRollUI _diceRollUI;
		[SerializeField]
		private RoundEndUI _roundEndUI;
		[SerializeField]
		private TableView _tableView;

		public static GameUIManager Instance { get; private set; }

		public void Awake()
		{
			if ( Instance != null )
			{
				Destroy( gameObject );
				return;
			}

			Instance = this;
		}

		private void Start()
		{
			_drawCardUI.Hide();
			_shiftTokenTargetSelectionUI.Hide();
			_discardCardUI.Hide();
			_roundNotificationUI.Hide();
			_shiftTokenNotificationUI.Hide();
			_disqualifiedNotificationUI.Hide();
			_gameOverNotificationUI.Hide();
			_diceRollUI.Hide();
			_roundEndUI.Hide();
		}

		public void InitializeUI()
		{
			Model.KesselSabacc game = GameplayManager.Instance.GameController.Model;

			_hud.Initialize( game, 0 );
			_tableView.Initialize( game );
		}

		public void HideAll()
		{

		}

		public void ShowDrawCardUI()
		{
			_drawCardUI.Show();
		}

		public void ShowDiscardCardUI()
		{

		}
	}
}
