using KesselSabacc.Model;
using TMPro;
using UnityEngine;

namespace KesselSabacc.UI.Components
{
	public class TurnCounterUI : UIComponent
	{
		[SerializeField]
		private TMP_Text _turnLabel;

		public void Initialize(KesselSabaccGameModel model)
		{
			SetTurn( model.CurrentTurn );
			model.OnRoundStart += SetTurn;
		}

		public void SetTurn(int value)
		{
			_turnLabel.SetText( $"{value}/3" );
		}
	}
}
