using KesselSabacc.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI.Components
{
	public class OpponentInfoPanel : UIComponent
	{
		[SerializeField]
		private ChipCounter _chipsView;
		[SerializeField]
		private ChipCounter _investedChipsView;
		[SerializeField]
		private Image _playerImage;
		[SerializeField]
		private TMP_Text _playerName;

		public void Initialize(Player player)
		{
			_chipsView.Initialize( player.Chips, player.Chips, true );
			_investedChipsView.Initialize( player.ChipsInvested, player.ChipsInvested, false );
			SetPlayerName( player.Name );

			player.OnChipsChanged += SetChips;
			player.OnChipsInvestedChanged += SetChipsInvested;
		}

		public void UpdateView(Player player)
		{
			SetChips( player.Chips );
			SetChipsInvested( player.ChipsInvested );
			SetPlayerName( player.Name );
		}

		public void SetChips(int value)
		{
			_chipsView.SetChips( value );
		}

		public void SetChipsInvested(int value)
		{
			_investedChipsView.SetChips( value );
		}

		public void SetPlayerName(string value)
		{
			_playerName.SetText( value );
		}

		public void SetPlayerSprite(Sprite sprite)
		{
			_playerImage.sprite = sprite;
		}
	}
}
