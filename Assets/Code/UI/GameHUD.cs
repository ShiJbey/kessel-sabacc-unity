using System;
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
		private ChipCounter _playerChips;
		[SerializeField]
		private ChipCounter _playerChipsInvested;
		[SerializeField]
		private OpponentUIRefs[] _opponentUI;

		private KesselSabaccGameView _gameView;

		public event Action OnDrawCardButtonClicked;
		public event Action OnStandButtonClicked;

		public void Initialize(KesselSabaccGameModel game, KesselSabaccGameView gameView, int playerIndex)
		{
			_gameView = gameView;
			_turnCounter.Initialize( game );

			var player = game.Players[playerIndex];
			_playerChips.Initialize( player );
			_playerChipsInvested.Initialize( player );

			player.OnChipsChanged += _playerChips.SetChips;
			player.OnChipsInvestedChanged += _playerChipsInvested.SetChips;

			int opponentUIIndex = 0;
			for ( int i = 0; i < game.Players.Count; i++ )
			{
				if ( i == playerIndex ) continue;

				_opponentUI[opponentUIIndex].infoPanel.Initialize( game.Players[i] );
				opponentUIIndex++;
			}

			for ( int i = opponentUIIndex; i < _opponentUI.Length; i++ )
			{
				_opponentUI[i].infoPanel.Hide();
			}

			HideActionButtons();
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

		public void HideActionButtons()
		{
			_drawButton.gameObject.SetActive( false );
			_standButton.gameObject.SetActive( false );
		}

		public void ShowActionButtons()
		{
			_drawButton.gameObject.SetActive( true );
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

		[System.Serializable]
		public class OpponentUIRefs
		{
			public OpponentInfoPanel infoPanel;
		}
	}
}
