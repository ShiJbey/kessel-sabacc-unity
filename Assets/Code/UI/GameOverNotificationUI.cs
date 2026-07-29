using System;
using KesselSabacc.Gameplay;
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

		private KesselSabaccGameController _gameController;

		public void Initialize(KesselSabaccGameController gameController)
		{
			_gameController = gameController;
		}

		protected override void Awake()
		{
			base.Awake();
			_messageTemplateString = _messageLabel.text;
		}

		protected override void SubscribeToEvents()
		{
			base.SubscribeToEvents();
			_continueButton.onClick.AddListener( OnContinueButtonClicked );
		}

		protected override void UnsubscribeFromEvents()
		{
			base.UnsubscribeFromEvents();
			_continueButton.onClick.RemoveListener( OnContinueButtonClicked );
		}

		public void ShowWinner(string name)
		{
			_messageLabel.SetText( _messageTemplateString.Replace( "#player#", name ) );
			Show();
		}

		private void OnContinueButtonClicked()
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();
			Hide();
			_gameController.GoToMainMenu();
		}
	}
}
