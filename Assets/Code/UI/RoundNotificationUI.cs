using TMPro;
using UnityEngine;

namespace KesselSabacc.UI
{
	public class RoundNotificationUI : UIComponent
	{
		[SerializeField]
		private TMP_Text _roundLabel;

		protected override void Awake()
		{
			base.Awake();
		}

		public void ShowRoundStartMessage(int round)
		{
			_roundLabel.text = $"Round {round} Start!";
		}

		public void ShowRoundEndMessage(int round)
		{
			_roundLabel.text = $"Round {round} Done!";
		}
	}
}
