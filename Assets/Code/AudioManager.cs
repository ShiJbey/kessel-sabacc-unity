using UnityEngine;
using UnityEngine.Audio;

namespace LoveHina
{
    /// <summary>
    /// Play
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public const string k_MasterGroup = "Master";
        public const string k_MusicGroup = "Music";
        public const string k_SfxGroup = "SFX";
        private const string k_Parameter = "_Volume";

        [SerializeField]
        private AudioMixer m_MainAudioMixer;

        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            GameSettings gameSettings = SettingsManager.Instance.Settings;
            OnSettingsUpdated(gameSettings);
        }

        void OnEnable()
        {
            SettingsManager.SettingsUpdated += OnSettingsUpdated;
        }

        void OnDisable()
        {
            SettingsManager.SettingsUpdated -= OnSettingsUpdated;
        }

        private void OnSettingsUpdated(GameSettings gameSettings)
        {
            // use the GameState to set the music and sfx volume
            SetVolume(k_MasterGroup + k_Parameter, gameSettings.masterVolume / 100f);
            SetVolume(k_MusicGroup + k_Parameter, gameSettings.musicVolume / 100f);
            SetVolume(k_SfxGroup + k_Parameter, gameSettings.sfxVolume / 100f);
        }

        // return an AudioMixerGroup by name
        public static AudioMixerGroup GetAudioMixerGroup(string groupName)
        {
            if (Instance == null)
                return null;

            if (Instance.m_MainAudioMixer == null)
                return null;

            AudioMixerGroup[] groups = Instance.m_MainAudioMixer.FindMatchingGroups(groupName);

            foreach (AudioMixerGroup match in groups)
            {
                if (match.ToString() == groupName)
                    return match;
            }
            return null;
        }

        // convert linear value between 0 and 1 to decibels
        public static float GetDecibelValue(float linearValue)
        {
            // commonly used for linear to decibel conversion
            float conversionFactor = 20f;

            float decibelValue = (linearValue != 0) ? conversionFactor * Mathf.Log10(linearValue) : -144f;
            return decibelValue;
        }

        // convert decibel value to a range between 0 and 1
        public static float GetLinearValue(float decibelValue)
        {
            float conversionFactor = 20f;

            return Mathf.Pow(10f, decibelValue / conversionFactor);

        }

        // converts linear value between 0 and 1 into decibels and sets AudioMixer level
        public static void SetVolume(string groupName, float linearValue)
        {
            float decibelValue = GetDecibelValue(linearValue);

            if (Instance.m_MainAudioMixer != null)
            {
                Instance.m_MainAudioMixer.SetFloat(groupName, decibelValue);
            }
        }

        public static float GetVolume(string groupName)
        {
            if (Instance == null)
                return 0f;

            float decibelValue = 0f;
            if (Instance.m_MainAudioMixer != null)
            {
                Instance.m_MainAudioMixer.GetFloat(groupName, out decibelValue);
            }
            return GetLinearValue(decibelValue);
        }

        public static void PlayOneSFX(AudioClip clip, Vector3 sfxPosition)
        {
            if (clip == null)
                return;

            GameObject sfxInstance = new GameObject(clip.name);
            sfxInstance.transform.position = sfxPosition;

            AudioSource source = sfxInstance.AddComponent<AudioSource>();
            source.clip = clip;

#if UNITY_WEBGL
			source.volume = GetVolume( k_SfxGroup + k_Parameter );
#else
            // set the mixer group (e.g. music, sfx, etc.)
            source.outputAudioMixerGroup = GetAudioMixerGroup(k_SfxGroup);
#endif

            source.Play();

            // destroy after clip length
            Destroy(sfxInstance, clip.length);
        }
    }
}
