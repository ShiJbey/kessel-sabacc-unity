using UnityEngine;
using UnityEngine.UI;

namespace Sabacc
{
	public class CreditsScreenController : UIComponent
	{
		[SerializeField]
		private Button m_BackButton;

		private MainMenuController m_MainMenuController;

		public void Initialize(MainMenuController mainMenuController)
		{
			m_MainMenuController = mainMenuController;
		}

		protected override void SubscribeToEvents()
		{
			m_BackButton.onClick.AddListener(HandleBackButtonClicked);
		}

		protected override void UnsubscribeFromEvents()
		{
			m_BackButton.onClick.RemoveListener(HandleBackButtonClicked);
		}

		private void HandleBackButtonClicked()
		{
			m_MainMenuController.PopScreen();
		}
	}
}
