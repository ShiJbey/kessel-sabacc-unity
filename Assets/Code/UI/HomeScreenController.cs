using UnityEngine;
using UnityEngine.UI;

namespace Sabacc
{
	public class HomeScreenController : UIComponent
	{
		[SerializeField]
		private Button m_PlayButton;

		[SerializeField]
		private Button m_CreditsButton;

		[SerializeField]
		private Button m_QuitButton;

		[SerializeField]
		private UIComponent m_CreditsScreen;

		private MainMenuController m_MainMenuController;

		public void Initialize(MainMenuController mainMenuController)
		{
			m_MainMenuController = mainMenuController;
		}

		protected override void SubscribeToEvents()
		{
			m_PlayButton.onClick.AddListener(HandlePlayButtonClicked);
			m_CreditsButton.onClick.AddListener(HandleCreditsButtonClicked);
			m_QuitButton.onClick.AddListener(HandleQuitButtonClicked);
		}

		protected override void UnsubscribeFromEvents()
		{
			m_PlayButton.onClick.RemoveListener(HandlePlayButtonClicked);
			m_CreditsButton.onClick.RemoveListener(HandleCreditsButtonClicked);
			m_QuitButton.onClick.AddListener(HandleQuitButtonClicked);
		}

		private void HandlePlayButtonClicked()
		{
			m_MainMenuController.StartNewGame();
		}

		private void HandleCreditsButtonClicked()
		{
			m_MainMenuController.PushScreen(m_CreditsScreen);
		}

		private void HandleQuitButtonClicked()
		{
#if UNITY_EDITOR
			// EditorApplication.isPlaying = false;
#endif

			Application.Quit();
		}
	}
}
