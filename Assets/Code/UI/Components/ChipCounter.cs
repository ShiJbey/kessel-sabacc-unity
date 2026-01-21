using System.Collections.Generic;
using KesselSabacc.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI.Components
{
	public class ChipCounter : UIComponent
	{
		[SerializeField]
		private Image _chipPrefab;
		[SerializeField]
		private RectTransform _chipsContainer;
		[SerializeField]
		private TMP_Text _valueLabel;
		[SerializeField]
		private bool _showChipsInvested;

		private Player _player;
		private List<Image> _chips = new();

		public void Initialize(Player player)
		{
			_player = player;

			if ( !_showChipsInvested )
			{
				_chipPrefab.gameObject.SetActive( false );

				for ( int i = 0; i < player.StartingChips; i++ )
				{
					var chip = Instantiate( _chipPrefab, _chipsContainer );
					chip.gameObject.SetActive( true );
					_chips.Add( chip );
				}
			}

			SetChips( player.Chips );
			if ( _showChipsInvested )
			{
				_player.OnChipsInvestedChanged += SetChips;
			}
			else
			{
				_player.OnChipsChanged += SetChips;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if ( _showChipsInvested && _player != null )
			{
				_player.OnChipsInvestedChanged -= SetChips;
			}
			else
			{
				_player.OnChipsChanged -= SetChips;
			}
		}

		public void SetChips(int value)
		{
			_valueLabel.SetText( value.ToString() );

			if ( !_showChipsInvested )
			{
				for ( int i = 0; i < _chips.Count; i++ )
				{
					if ( i >= value )
					{
						var chip = _chips[i];
						chip.color = new Color( chip.color.r, chip.color.b, chip.color.g, 0.5f );
					}
				}
			}
		}
	}
}
