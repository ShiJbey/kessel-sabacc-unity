using KesselSabacc.UI.Components;
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

		public void Initialize(Model.KesselSabacc game, int playerIndex)
		{
			_turnCounter.Initialize( game );

			var player = game.Players[playerIndex];
			_playerChips.Initialize( player.Chips, player.Chips, true );
			_playerChipsInvested.Initialize( player.ChipsInvested, player.ChipsInvested, false );

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
		}

		protected override void SubscribeToEvents()
		{
			_drawButton.onClick.AddListener( OnDrawButtonClicked );
			_standButton.onClick.AddListener( OnStandButtonClicked );
		}

		protected override void UnsubscribeFromEvents()
		{
			_drawButton.onClick.RemoveListener( OnDrawButtonClicked );
			_standButton.onClick.RemoveListener( OnStandButtonClicked );
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

		private void OnDrawButtonClicked()
		{
			GameUIManager.Instance.ShowDrawCardUI();
		}

		private void OnStandButtonClicked()
		{

		}


		[System.Serializable]
		public class OpponentUIRefs
		{
			public OpponentInfoPanel infoPanel;
		}
	}
}
