using System;
using UnityEngine;

namespace LoveHina
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
        private GameSettings settings;

        /// <summary>
        /// The current SettingsManager singleton instance.
        /// </summary>
        public static SettingsManager Instance { get; private set; }

        public GameSettings Settings => settings;

        public static Action<GameSettings> SettingsUpdated;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }

        private void Start()
        {
            // Attempt to load settings from PlayerPrefs
            // If it succeeds, update the settings with those loaded in
            // otherwise, just pass a new settings object.
            GameSettings playerPrefSettings = LoadSettingsFromPlayerPrefs();
            if (playerPrefSettings != null)
            {
                UpdateSettings(playerPrefSettings);
            }
            else
            {
                UpdateSettings(new GameSettings());
            }
        }


        public static void UpdateSettings(GameSettings settings)
        {
            Instance.settings = new GameSettings(settings);
            Instance.SaveSettingsToPlayerPrefs();
            SettingsUpdated?.Invoke(Instance.settings);
        }

        private void SaveSettingsToPlayerPrefs()
        {
            string settingsJson = JsonUtility.ToJson(settings);
            PlayerPrefs.SetString(k_SettingsPlayerPrefKey, settingsJson);
        }

        /// <summary>
        /// Load a GameSettings instance from PlayerPrefs
        /// </summary>
        /// <returns>GameSettings or null if none found in PlayerPrefs.</returns>
        private GameSettings LoadSettingsFromPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(k_SettingsPlayerPrefKey))
            {
                string settingsJson = PlayerPrefs.GetString(k_SettingsPlayerPrefKey);
                GameSettings gameSettings = JsonUtility.FromJson<GameSettings>(settingsJson);
                return gameSettings;
            }

            return null;
        }
    }
}
