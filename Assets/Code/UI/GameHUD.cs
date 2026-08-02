using System;
using KesselSabacc.Gameplay;
using KesselSabacc.Model;
using KesselSabacc.UI.Components;
using KesselSabacc.Views;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI
{
	public class GameHUD : UIComponent
	{
		[SerializeField]
		private Button _drawButton;
		[SerializeField]
		private Button _standButton;
		[SerializeField]
		private TurnCounterUI _turnCounter;
		[SerializeField]
		private RemainingChipCounter _playerChipsRemaining;
		[SerializeField]
		private InvestedChipCounter _playerChipsInvested;
		[SerializeField]
		private OpponentUIRefs[] _opponentUI;

		private Player _player;

		public event Action OnDrawCardButtonClicked;
		public event Action OnStandButtonClicked;

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if ( _player != null )
			{
				_player.OnChipsChanged -= OnPlayerChipsChanged;
				_player.OnChipsInvestedChanged -= OnPlayerChipsInvestedChanged;
				_player = null;
			}
		}

		public void Initialize(KesselSabaccGameController gameController, int playerIndex)
		{
			_turnCounter.Initialize( gameController.Model );

			_player = gameController.Players[playerIndex].Model;

			_playerChipsRemaining.SetCurrentChipCount( _player.Chips );
			_playerChipsInvested.SetChipCount( 0 );

			_player.OnChipsChanged += OnPlayerChipsChanged;
			_player.OnChipsInvestedChanged += OnPlayerChipsInvestedChanged;

			int opponentUIIndex = 0;
			for ( int i = 0; i < gameController.Players.Count; i++ )
			{
				if ( i == playerIndex ) continue;

				_opponentUI[opponentUIIndex].infoPanel.Initialize( gameController.Players[i], gameController.playerColors[i] );
				opponentUIIndex++;
			}

			for ( int i = opponentUIIndex; i < _opponentUI.Length; i++ )
			{
				_opponentUI[i].infoPanel.Hide();
			}

			HideDrawButton();
			HideStandButton();
		}

		protected override void SubscribeToEvents()
		{
			_drawButton.onClick.AddListener( HandleDrawButtonClicked );
			_standButton.onClick.AddListener( HandleStandButtonClicked );
		}

		protected override void UnsubscribeFromEvents()
		{
			_drawButton.onClick.RemoveListener( HandleDrawButtonClicked );
			_standButton.onClick.RemoveListener( HandleStandButtonClicked );
		}

		public void HideDrawButton()
		{
			_drawButton.gameObject.SetActive( false );
		}

		public void ShowDrawButton()
		{
			_drawButton.gameObject.SetActive( true );
		}

		public void HideStandButton()
		{
			_standButton.gameObject.SetActive( false );
		}

		public void ShowStandButton()
		{
			_standButton.gameObject.SetActive( true );
		}

		private void HandleDrawButtonClicked()
		{
			OnDrawCardButtonClicked?.Invoke();
		}

		private void HandleStandButtonClicked()
		{
			OnStandButtonClicked?.Invoke();
		}

		private void OnPlayerChipsChanged(int chips)
		{
			_playerChipsRemaining.SetCurrentChipCount( chips );
		}

		private void OnPlayerChipsInvestedChanged(int chips)
		{
			_playerChipsInvested.SetChipCount( chips );
		}

		[System.Serializable]
		public class OpponentUIRefs
		{
			public OpponentInfoPanel infoPanel;
		}
	}
}
