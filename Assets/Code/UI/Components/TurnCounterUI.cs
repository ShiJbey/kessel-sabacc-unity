using TMPro;
using UnityEngine;

namespace KesselSabacc.UI.Components
{
	public class TurnCounterUI : UIComponent
	{
		[SerializeField]
		private TMP_Text _turnLabel;

		public void Initialize(Model.KesselSabacc game)
		{
			SetTurn( game.CurrentTurn );
			game.OnRoundStart += SetTurn;
		}

		public void SetTurn(int value)
		{
			_turnLabel.SetText( $"{value}/3" );
		}
	}
}
