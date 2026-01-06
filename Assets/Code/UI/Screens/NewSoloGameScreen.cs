using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KesselSabacc.UI.Screens
{
	public class NewSoloGameScreen : UIComponent
	{
		[SerializeField]
		private TMP_Dropdown _numPlayersDropdown;
		[SerializeField]
		private TMP_Dropdown _numChipsDropdown;
		[SerializeField]
		private Button _backButton;
		[SerializeField]
		private Button _playButton;

		private MainMenuController _mainMenuController;

		public void Initialize(MainMenuController mainMenuController)
		{
			_mainMenuController = mainMenuController;
		}

		public override void Show()
		{
			base.Show();
			NewGameManager.Instance.CreateNewGame();
			UpdateValues( NewGameManager.Instance.Data );
		}

		public void UpdateValues(NewGameData data)
		{
			if ( data == null ) return;

			switch ( data.numPlayers )
			{
				case 2:
					_numPlayersDropdown.SetValueWithoutNotify( 0 );
					break;
				case 3:
					_numPlayersDropdown.SetValueWithoutNotify( 1 );
					break;
				case 4:
					_numPlayersDropdown.SetValueWithoutNotify( 2 );
					break;
			}

			switch ( data.numChips )
			{
				case 4:
					_numChipsDropdown.SetValueWithoutNotify( 0 );
					break;
				case 5:
					_numChipsDropdown.SetValueWithoutNotify( 1 );
					break;
				case 6:
					_numChipsDropdown.SetValueWithoutNotify( 2 );
					break;
				case 7:
					_numChipsDropdown.SetValueWithoutNotify( 3 );
					break;
				case 8:
					_numChipsDropdown.SetValueWithoutNotify( 4 );
					break;
			}
		}

		protected override void SubscribeToEvents()
		{
			_numPlayersDropdown.onValueChanged.AddListener( OnNumPlayersChanged );
			_numChipsDropdown.onValueChanged.AddListener( OnNumChipsChanged );
			_backButton.onClick.AddListener( OnBackButtonClicked );
			_playButton.onClick.AddListener( OnPlayButtonClicked );
		}

		protected override void UnsubscribeFromEvents()
		{
			_numPlayersDropdown.onValueChanged.RemoveListener( OnNumPlayersChanged );
			_numChipsDropdown.onValueChanged.RemoveListener( OnNumChipsChanged );
			_backButton.onClick.RemoveListener( OnBackButtonClicked );
			_playButton.onClick.RemoveListener( OnPlayButtonClicked );
		}

		private void OnNumPlayersChanged(int choiceIndex)
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();
			string choiceLabel = _numPlayersDropdown.options[choiceIndex].text;
			int numPlayers = int.Parse( choiceLabel );
			NewGameManager.Instance.SetNumPlayers( numPlayers );
		}

		private void OnNumChipsChanged(int choiceIndex)
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();
			string choiceLabel = _numChipsDropdown.options[choiceIndex].text;
			int numChips = int.Parse( choiceLabel );
			NewGameManager.Instance.SetNumChips( numChips );
		}

		private void OnBackButtonClicked()
		{
			NewGameManager.Instance.ClearNewGame();
			_mainMenuController.ShowHomeScreen();
		}

		private void OnPlayButtonClicked()
		{
			LoadingScreen loadingScreen = FindFirstObjectByType<LoadingScreen>( FindObjectsInactive.Include );
			loadingScreen?.Show();
			SceneManager.LoadScene( "Scenes/SoloMatch" );
		}
	}
}
