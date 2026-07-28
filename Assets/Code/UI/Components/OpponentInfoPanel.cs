using KesselSabacc.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI.Components
{
	public class OpponentInfoPanel : UIComponent
	{
		[SerializeField]
		private RemainingChipCounter _chipsView;
		[SerializeField]
		private InvestedChipCounter _investedChipsView;
		[SerializeField]
		private Image _playerImage;
		[SerializeField]
		private TMP_Text _playerName;

		private Player _player;

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (_player != null)
			{
				_player.OnChipsChanged -= OnPlayerChipsChanged;
				_player.OnChipsInvestedChanged -= OnPlayerChipsInvestedChanged;
				_player = null;
			}
		}

		public void Initialize(Player player)
		{
			_player = player;
			_chipsView.SetMaxChipCount(player.StartingChips);
			_chipsView.SetCurrentChipCount(player.Chips);
			_investedChipsView.SetChipCount(0);
			_player.OnChipsChanged += OnPlayerChipsChanged;
			_player.OnChipsInvestedChanged += OnPlayerChipsInvestedChanged;
			SetPlayerName( player.Name );
		}

		public void UpdateView(Player player)
		{
			SetPlayerName( player.Name );
		}

		public void SetPlayerName(string value)
		{
			_playerName.SetText( value );
		}

		public void SetPlayerSprite(Sprite sprite)
		{
			_playerImage.sprite = sprite;
		}

		private void OnPlayerChipsChanged(int chips)
		{
			_chipsView.SetCurrentChipCount(chips);
		}

		private void OnPlayerChipsInvestedChanged(int chips)
		{
			_investedChipsView.SetChipCount(chips);
		}
	}
}
