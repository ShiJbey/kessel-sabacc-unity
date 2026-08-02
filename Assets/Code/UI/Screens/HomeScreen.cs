using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KesselSabacc.UI.Screens
{
	/// <summary>
	/// Manages the presentation of the HomeScreen.
	/// </summary>
	public class HomeScreen : UIComponent
	{
		[SerializeField]
		private Button _playButton;

		[SerializeField]
		private Button _creditsButton;

		[SerializeField]
		private Button _settingsButton;

		[SerializeField]
		private Button _exitButton;

		[SerializeField]
		private TMP_Text _versionText;

		private MainMenuController _mainMenuController;

		private void Start()
		{
			EventSystem.current.firstSelectedGameObject = _playButton.gameObject;
			_versionText.SetText(Application.version);
		}

		public void Initialize(MainMenuController mainMenuController)
		{
			_mainMenuController = mainMenuController;
		}

		protected override void SubscribeToEvents()
		{
			_playButton.onClick.AddListener( HandlePlayButtonClicked );
			_creditsButton.onClick.AddListener( HandleCreditsButtonClicked );
			_settingsButton.onClick.AddListener( HandleSettingsButtonClicked );
			_exitButton.onClick.AddListener( HandleExitButtonClicked );
		}

		protected override void UnsubscribeFromEvents()
		{
			_playButton.onClick.RemoveListener( HandlePlayButtonClicked );
			_creditsButton.onClick.RemoveListener( HandleCreditsButtonClicked );
			_settingsButton.onClick.RemoveListener( HandleSettingsButtonClicked );
			_exitButton.onClick.AddListener( HandleExitButtonClicked );
		}

		private void HandlePlayButtonClicked()
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();
			_mainMenuController.ShowSoloGameScreen();
		}

		private void HandleCreditsButtonClicked()
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();
			_mainMenuController.ShowCreditsScreen();
		}

		private void HandleSettingsButtonClicked()
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();
			_mainMenuController.ShowSettingsScreen();
		}

		private void HandleExitButtonClicked()
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();

#if UNITY_STANDALONE
			Application.Quit();
#endif

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}
	}
}
