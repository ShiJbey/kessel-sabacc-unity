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
			_chipsView.Initialize( player );
			_investedChipsView.Initialize( player );
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
	}
}
