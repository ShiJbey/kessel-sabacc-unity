using System.Collections;
using KesselSabacc.UI.Screens;
using UnityEngine;

namespace KesselSabacc.UI
{
	public class MainMenuController : MonoBehaviour
	{
		[SerializeField]
		private HomeScreen _homeScreen;

		[SerializeField]
		private CreditsScreen _creditsScreen;

		[SerializeField]
		private NewSoloGameScreen _newSoloGameScreen;

		[SerializeField]
		private SettingsScreen _settingsScreen;

		private UIComponent _currentScreen = null;

		private void Start()
		{
			StartCoroutine( Initialize() );
		}

		private IEnumerator Initialize()
		{
			yield return new WaitUntil( () => AutoLoadManager.Instance.isReady );

			var loadingScreen = FindFirstObjectByType<LoadingScreen>( FindObjectsInactive.Include );
			loadingScreen.Show();

			_homeScreen.Initialize( this );
			_creditsScreen.Initialize( this );
			_newSoloGameScreen.Initialize( this );
			_settingsScreen.Initialize( this );

			_homeScreen.Hide();
			_creditsScreen.Hide();
			_newSoloGameScreen.Hide();
			_settingsScreen.Hide();

			ShowHomeScreen();
			loadingScreen.Hide();
		}

		public void ShowHomeScreen()
		{
			_currentScreen?.Hide();
			_currentScreen = _homeScreen;
			_currentScreen.Show();
		}

		public void ShowCreditsScreen()
		{
			_currentScreen?.Hide();
			_currentScreen = _creditsScreen;
			_currentScreen.Show();
		}

		public void ShowSoloGameScreen()
		{
			_currentScreen?.Hide();
			_currentScreen = _newSoloGameScreen;
			_currentScreen.Show();
		}

		public void ShowSettingsScreen()
		{
			_currentScreen?.Hide();
			_currentScreen = _settingsScreen;
			_currentScreen.Show();
		}
	}
}
