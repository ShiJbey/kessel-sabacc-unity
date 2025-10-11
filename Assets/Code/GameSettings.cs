using System;

namespace LoveHina
{
    [Serializable]
    public class GameSettings
    {
        public int masterVolume;
        public int sfxVolume;
        public int musicVolume;
        public DisplayMode displayMode;

        public bool vSyncEnabled;

        public GameSettings()
        {
            masterVolume = 100;
            sfxVolume = 100;
            musicVolume = 100;
            displayMode = DisplayMode.FullScreen;
            vSyncEnabled = true;
        }

        public GameSettings(GameSettings original)
        {
            masterVolume = original.masterVolume;
            sfxVolume = original.sfxVolume;
            musicVolume = original.musicVolume;
            displayMode = original.displayMode;
            vSyncEnabled = original.vSyncEnabled;
        }

        public enum DisplayMode
        {
            FullScreen = 0,
            Windowed = 1,
        }
    }
}
