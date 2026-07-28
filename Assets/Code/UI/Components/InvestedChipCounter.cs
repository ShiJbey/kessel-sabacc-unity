using TMPro;
using UnityEngine;

namespace KesselSabacc.UI.Components
{
	public class InvestedChipCounter : UIComponent
	{
		[SerializeField]
		private TMP_Text _valueLabel;

		public void SetChipCount(int value)
		{
			_valueLabel.SetText(value.ToString());
		}
	}
}
