using System;
using UnityEngine;

namespace KesselSabacc
{
	public class NewGameManager : MonoBehaviour
	{
		[SerializeField]
		private NewGameData m_Data;

		public static NewGameManager Instance { get; private set; }

		public static event Action<NewGameData> OnDataChanged;

		public NewGameData Data
		{
			get => m_Data;
			set => m_Data = value;
		}

		public void CreateNewGame()
		{
			Data = new NewGameData();
		}

		public void SetNumPlayers(int value)
		{
			m_Data.numPlayers = value;
			OnDataChanged?.Invoke( m_Data );
		}

		public void SetNumChips(int value)
		{
			m_Data.numChips = value;
			OnDataChanged?.Invoke( m_Data );
		}

		public void ClearNewGame()
		{
			Data = null;
		}

		private void Awake()
		{
			if ( Instance != null )
			{
				Destroy( this );
				return;
			}

			Instance = this;
		}
	}
}
