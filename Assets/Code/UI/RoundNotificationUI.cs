using TMPro;
using UnityEngine;

namespace KesselSabacc.UI
{
	public class RoundNotificationUI : UIComponent
	{
		[SerializeField]
		private TMP_Text _roundLabel;

		private string _roundTextTemplate;

		protected override void Awake()
		{
			base.Awake();
			_roundTextTemplate = _roundLabel.text;
		}

		public void SetRound(int round)
		{
			_roundLabel.text = _roundTextTemplate.Replace( "#round-number#", round.ToString() );
		}
	}
}
