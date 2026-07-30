using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using KesselSabacc.Gameplay;
using KesselSabacc.Gameplay.PlayerActions;
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

		private PlayerAction _standAction;
		private Action<PlayerAction> _standCallback;

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

		private void OnEnable()
		{
			hud.OnDrawCardButtonClicked += OnDrawCardButtonClicked;
			hud.OnStandButtonClicked += OnStandButtonClicked;
		}

		private void OnDisable()
		{
			hud.OnDrawCardButtonClicked -= OnDrawCardButtonClicked;
			hud.OnStandButtonClicked -= OnStandButtonClicked;
		}

		public void Initialize(KesselSabaccGameController gameController)
		{
			hud.Initialize( gameController.Model, 0 );
			tableView.Initialize( gameController );
			roundEndUI.Initialize( gameController );
			gameOverNotificationUI.Initialize( gameController );
		}

		public void PresentActionUI(IReadOnlyList<PlayerAction> legalActions, Action<PlayerAction> onChosen)
		{
			Dictionary<ActionType, IReadOnlyList<PlayerAction>> actionsByType = legalActions
				.GroupBy( a => a.ActionType )
				.ToDictionary( g => g.Key, g => (IReadOnlyList<PlayerAction>)g.ToList() );

			if ( actionsByType.ContainsKey( ActionType.DISCARD_CARD ) )
			{
				discardCardUI.UpdateView( actionsByType[ActionType.DISCARD_CARD].Cast<DiscardCardAction>().ToImmutableList(), onChosen );
				discardCardUI.Show();
				return;
			}

			if ( actionsByType.ContainsKey( ActionType.DRAW_CARD ) )
			{
				drawCardUI.UpdateView( actionsByType[ActionType.DRAW_CARD].Cast<DrawCardAction>().ToImmutableList(), onChosen );
				hud.ShowDrawButton();
			}

			if ( actionsByType.ContainsKey( ActionType.STAND ) )
			{
				_standAction = actionsByType[ActionType.STAND][0];
				_standCallback = onChosen;
				hud.ShowStandButton();
			}
		}

		public void HideAllActionUI()
		{
			drawCardUI.Hide();
			shiftTokenTargetSelectionUI.Hide();
			discardCardUI.Hide();
			diceRollUI.Hide();
			hud.HideDrawButton();
			hud.HideStandButton();
		}

		private void OnDrawCardButtonClicked()
		{
			drawCardUI.Show();
		}

		private void OnStandButtonClicked()
		{
			_standCallback?.Invoke( _standAction );
			_standAction = null;
			_standCallback = null;
		}
	}
}
