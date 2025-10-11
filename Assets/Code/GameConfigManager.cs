using UnityEngine;

namespace LoveHina
{
	public class GameConfigManager : MonoBehaviour
	{
		[SerializeField]
		private GameConfig m_Config;

		public GameConfig Config => m_Config;

		public static GameConfigManager Instance { get; private set; }

		private void Awake()
		{
			if ( Instance != null )
			{
				Destroy( gameObject );
				return;
			}

			Instance = this;
		}
	}
}
