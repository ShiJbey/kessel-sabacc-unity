using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI
{
	public class GameOverNotificationUI : UIComponent
	{
		[SerializeField]
		private TMP_Text _messageLabel;
		[SerializeField]
		private Button _continueButton;

		private string _messageTemplateString;

		public event Action OnContinue;

		protected override void Awake()
		{
			base.Awake();
			_messageTemplateString = _messageLabel.text;
		}

		protected override void SubscribeToEvents()
		{
			base.SubscribeToEvents();
			_continueButton.onClick.AddListener( Continue );
		}

		protected override void UnsubscribeFromEvents()
		{
			base.UnsubscribeFromEvents();
			_continueButton.onClick.RemoveListener( Continue );
		}

		public void SetPlayerName(string name)
		{
			_messageLabel.SetText( _messageTemplateString.Replace( "#player#", name ) );
		}

		public void Continue()
		{
			OnContinue?.Invoke();
		}
	}
}
