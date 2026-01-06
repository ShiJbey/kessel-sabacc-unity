using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI.Screens
{
	public class SettingsScreen : UIComponent
	{
		[SerializeField]
		private Slider _masterVolumeSlider;
		[SerializeField]
		private Slider _sfxVolumeSlider;
		[SerializeField]
		private Slider _musicVolumeSlider;
		[SerializeField]
		private TMP_Dropdown _displayModeDropdown;
		[SerializeField]
		private TMP_Dropdown _vSyncDropdown;
		[SerializeField]
		private Button _backButton;
		[SerializeField]
		private Button _resetButton;

		private MainMenuController _mainMenuController;

		public void Initialize(MainMenuController mainMenuController)
		{
			_mainMenuController = mainMenuController;
		}

		public override void Show()
		{
			base.Show();
			UpdateValues( SettingsManager.Instance.Settings );
		}

		public void UpdateValues(GameSettings settings)
		{
			_masterVolumeSlider.SetValueWithoutNotify( settings.masterVolume );
			_musicVolumeSlider.SetValueWithoutNotify( settings.musicVolume );
			_sfxVolumeSlider.SetValueWithoutNotify( settings.sfxVolume );

			switch ( settings.fullScreenMode )
			{
				case FullScreenMode.FullScreenWindow:
					_displayModeDropdown.SetValueWithoutNotify( 0 );
					break;
				case FullScreenMode.Windowed:
					_displayModeDropdown.SetValueWithoutNotify( 1 );
					break;
			}

			_vSyncDropdown.SetValueWithoutNotify( settings.vSyncEnabled ? 0 : 1 );
		}

		protected override void SubscribeToEvents()
		{
			_masterVolumeSlider.onValueChanged.AddListener( HandleMasterVolumeChanged );
			_sfxVolumeSlider.onValueChanged.AddListener( HandleSFXVolumeChanged );
			_musicVolumeSlider.onValueChanged.AddListener( HandleMusicVolumeChanged );
			_displayModeDropdown.onValueChanged.AddListener( HandleFullScreenModeChanged );
			_vSyncDropdown.onValueChanged.AddListener( HandleVSyncModeChanged );
			_backButton.onClick.AddListener( OnBackButtonClicked );
			_resetButton.onClick.AddListener( OnResetButtonClicked );
		}

		protected override void UnsubscribeFromEvents()
		{
			_masterVolumeSlider.onValueChanged.RemoveListener( HandleMasterVolumeChanged );
			_sfxVolumeSlider.onValueChanged.RemoveListener( HandleSFXVolumeChanged );
			_musicVolumeSlider.onValueChanged.RemoveListener( HandleMusicVolumeChanged );
			_displayModeDropdown.onValueChanged.RemoveListener( HandleFullScreenModeChanged );
			_vSyncDropdown.onValueChanged.RemoveListener( HandleVSyncModeChanged );
			_backButton.onClick.RemoveListener( OnBackButtonClicked );
			_resetButton.onClick.RemoveListener( OnResetButtonClicked );

		}

		public void HandleMasterVolumeChanged(float value)
		{
			SettingsManager.Instance.SetMasterVolume( (int)value );
		}

		public void HandleMusicVolumeChanged(float value)
		{
			SettingsManager.Instance.SetMusicVolume( (int)value );
		}

		public void HandleSFXVolumeChanged(float value)
		{
			SettingsManager.Instance.SetSFXVolume( (int)value );
		}

		public void MasterVolumeSlider_OnPointerUp()
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();
		}

		public void SFXVolumeSlider_OnPointerUp()
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();
		}

		public void MusicVolumeSlider_OnPointerUp()
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();
		}

		public void HandleFullScreenModeChanged(int choiceIndex)
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();
			string choiceLabel = _displayModeDropdown.options[choiceIndex].text;
			switch ( choiceLabel )
			{
				case "Full Screen":
					SettingsManager.Instance.SetFullScreenMode( FullScreenMode.FullScreenWindow );
					break;
				case "Windowed":
					SettingsManager.Instance.SetFullScreenMode( FullScreenMode.MaximizedWindow );
					break;
				default:
					throw new System.ArgumentException( "Unsupported dropdown choice given: " + choiceLabel );
			}
		}

		public void HandleVSyncModeChanged(int choiceIndex)
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();
			string choiceLabel = _vSyncDropdown.options[choiceIndex].text;
			switch ( choiceLabel )
			{
				case "Enabled":
					SettingsManager.Instance.SetVSyncEnabled( true );
					break;
				case "Disabled":
					SettingsManager.Instance.SetVSyncEnabled( false );
					break;
				default:
					throw new System.ArgumentException( "Unsupported dropdown choice given: " + choiceLabel );
			}
		}

		private void OnResetButtonClicked()
		{
			SettingsManager.Instance.Reset();
			UpdateValues( SettingsManager.Instance.Settings );
		}

		private void OnBackButtonClicked()
		{
			SettingsManager.Instance.SaveSettings();
			_mainMenuController.ShowHomeScreen();
		}
	}
}
