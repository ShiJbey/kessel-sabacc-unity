using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI.Screens
{
	public class CreditsScreen : UIComponent
	{
		[SerializeField]
		private Button _backButton;

		private MainMenuController _mainMenuController;

		public void Initialize(MainMenuController mainMenuController)
		{
			_mainMenuController = mainMenuController;
		}

		protected override void SubscribeToEvents()
		{
			_backButton.onClick.AddListener(HandleBackButtonClicked);
		}

		protected override void UnsubscribeFromEvents()
		{
			_backButton.onClick.RemoveListener(HandleBackButtonClicked);
		}

		private void HandleBackButtonClicked()
		{
			_mainMenuController.ShowHomeScreen();
		}
	}
}
