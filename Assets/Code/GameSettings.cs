using System;
using UnityEngine;

namespace KesselSabacc
{
	[Serializable]
	public class GameSettings
	{
		public int masterVolume = 100;
		public int sfxVolume = 100;
		public int musicVolume = 100;
		public FullScreenMode fullScreenMode = FullScreenMode.FullScreenWindow;
		public bool vSyncEnabled = true;

		public GameSettings()
		{

		}

		public GameSettings(GameSettings original)
		{
			masterVolume = original.masterVolume;
			sfxVolume = original.sfxVolume;
			musicVolume = original.musicVolume;
			fullScreenMode = original.fullScreenMode;
			vSyncEnabled = original.vSyncEnabled;
		}
	}
}
