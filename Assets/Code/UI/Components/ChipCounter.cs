using System.Collections.Generic;
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

		private List<Image> _chips = new();

		public void Initialize(int currentValue, int maxChips, bool showMultipleChips)
		{
			if ( showMultipleChips )
			{
				_chipPrefab.gameObject.SetActive( false );

				for ( int i = 0; i < maxChips; i++ )
				{
					var chip = Instantiate( _chipPrefab, _chipsContainer );
					chip.gameObject.SetActive( true );
					_chips.Add( chip );
				}
			}

			SetChips( currentValue );
		}

		public void SetChips(int value)
		{
			_valueLabel.SetText( value.ToString() );

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
