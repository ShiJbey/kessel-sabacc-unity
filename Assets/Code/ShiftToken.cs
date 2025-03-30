using UnityEngine;

namespace Sabacc
{
    public class ShiftToken
    {
        private ShiftTokenData m_Data;

		private bool m_IsUsed;

        public string Name => m_Data.Name;

        public string Description => m_Data.Description;

        public Sprite Sprite => m_Data.Sprite;

		public bool IsUsed
		{
			get => m_IsUsed;
			set => m_IsUsed = value;
		}

        public ShiftToken(ShiftTokenData data)
        {
            m_Data = data;
        }
    }
}

