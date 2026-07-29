using TMPro;
using UnityEngine;

namespace KesselSabacc.UI
{
	public class RoundNotificationUI : UIComponent
	{
		[SerializeField]
		private TMP_Text _roundLabel;
		[SerializeField]
		private float _animationDuration = 2f;

		protected override void Awake()
		{
			base.Awake();
		}

		public async Awaitable PlayRoundStartAnim(int round)
		{
			_roundLabel.text = $"Round {round} Start!";
			Show();
			await Awaitable.WaitForSecondsAsync( _animationDuration );
			Hide();
		}

		public async Awaitable PlayRoundEndAnim(int round)
		{
			_roundLabel.text = $"Round {round} Done!";
			Show();
			await Awaitable.WaitForSecondsAsync( _animationDuration );
			Hide();
		}
	}
}
