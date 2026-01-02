using UnityEngine;

namespace KesselSabacc
{
    public class NewGameManager : MonoBehaviour
    {
        [SerializeField]
        private NewGameData m_Data;

        [SerializeField]
        private bool m_OverrideGameData;

        public static NewGameManager Instance { get; private set; }

        public NewGameData Data
        {
            get => m_Data;
            set => m_Data = value;
        }

        public bool overrideGameData
        {
            get => m_OverrideGameData;
            set => m_OverrideGameData = value;
        }

        private void Awake()
        {
            if ( Instance != null )
            {
                Destroy( this );
                return;
            }

            Instance = this;
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            GameConfig gameConfig = GameConfigManager.Instance.Config;
            m_Data.playerName = "";
            m_Data.startingChips = gameConfig.startingChips;
            m_Data.numberOfPlayers = gameConfig.numberOfPlayers;
        }
    }
}
