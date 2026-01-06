using System;
using UnityEngine;

namespace KesselSabacc
{
	public class SettingsManager : MonoBehaviour
	{
		/// <summary>
		/// The key used to store settings JSON within PlayerPrefs.
		/// </summary>
		private const string k_SettingsPlayerPrefKey = "settings";

		/// <summary>
		/// Toggle if the SettingsManager should be destroyed when loading a new scene.
		/// </summary>
		[SerializeField]
		private bool dontDestroyOnLoad;

		/// <summary>
		/// The current settings values.
		/// </summary>
		[SerializeField]
		private GameSettings _settings;

		/// <summary>
		/// The current SettingsManager singleton instance.
		/// </summary>
		public static SettingsManager Instance { get; private set; }

		public GameSettings Settings => _settings;

		public static Action<GameSettings> OnSettingsChanged;

		private void Awake()
		{
			if ( Instance != null )
			{
				Destroy( gameObject );
				return;
			}

			Instance = this;

			// Attempt to load settings from PlayerPrefs
			// If it succeeds, update the settings with those loaded in
			// otherwise, just pass a new settings object.
			GameSettings playerPrefSettings = LoadSettingsFromPlayerPrefs();
			if ( playerPrefSettings != null )
			{
				SetSettings( playerPrefSettings );
			}
			else
			{
				SetSettings( new GameSettings() );
			}

			if ( dontDestroyOnLoad )
			{
				DontDestroyOnLoad( this );
			}
		}

		public void SetMasterVolume(int value)
		{
			if ( _settings.masterVolume != value )
			{
				_settings.masterVolume = value;
				OnSettingsChanged?.Invoke( _settings );
			}
		}

		public void SetSFXVolume(int value)
		{
			if ( _settings.sfxVolume != value )
			{
				_settings.sfxVolume = value;
				OnSettingsChanged?.Invoke( _settings );
			}
		}

		public void SetMusicVolume(int value)
		{
			if ( _settings.musicVolume != value )
			{
				_settings.musicVolume = value;
				OnSettingsChanged?.Invoke( _settings );
			}
		}

		public void SetFullScreenMode(FullScreenMode mode)
		{
			if ( mode != _settings.fullScreenMode )
			{
				_settings.fullScreenMode = mode;
				Screen.fullScreenMode = mode;
				OnSettingsChanged?.Invoke( _settings );
			}
		}

		public void SetVSyncEnabled(bool isEnabled)
		{
			if ( _settings.vSyncEnabled != isEnabled )
			{
				_settings.vSyncEnabled = isEnabled;
				OnSettingsChanged?.Invoke( _settings );
			}
		}


		private void SetSettings(GameSettings settings)
		{
			_settings = new GameSettings( settings );
			SetFullScreenMode( settings.fullScreenMode );
			SaveSettings();
			OnSettingsChanged?.Invoke( _settings );
		}

		public void SaveSettings()
		{
			string settingsJson = JsonUtility.ToJson( _settings );
			PlayerPrefs.SetString( k_SettingsPlayerPrefKey, settingsJson );
		}

		public void Reset()
		{
			SetSettings( new GameSettings() );
		}

		/// <summary>
		/// Load a GameSettings instance from PlayerPrefs
		/// </summary>
		/// <returns>GameSettings or null if none found in PlayerPrefs.</returns>
		private GameSettings LoadSettingsFromPlayerPrefs()
		{
			if ( PlayerPrefs.HasKey( k_SettingsPlayerPrefKey ) )
			{
				string settingsJson = PlayerPrefs.GetString( k_SettingsPlayerPrefKey );
				GameSettings gameSettings = JsonUtility.FromJson<GameSettings>( settingsJson );
				return gameSettings;
			}

			return null;
		}
	}
}
