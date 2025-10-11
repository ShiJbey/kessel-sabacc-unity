using UnityEngine;

namespace Sabacc
{
	/// <summary>
	/// Manages the flow of a game of KesselSabacc
	/// </summary>
	public class SabaccMatchController : MonoBehaviour
	{
		private SabaccMatch m_Match;

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{
			InitializeMatch();
			ResetDecks();
			DealHands();
		}

		// Update is called once per frame
		void Update()
		{

		}

		private void InitializeMatch()
		{
			Debug.Log( "Initializing Match" );
			m_Match = new SabaccMatch(4, 4);
		}

		/// <summary>
		/// Reset the cards within the blood and sand decks, clear swap stacks.
		/// </summary>
		public void ResetDecks()
		{
			Debug.Log( "Reseting Blood and Sand decks." );
			m_Match.ResetDecks();
		}

		/// <summary>
		/// Deal hands to the players.
		/// </summary>
		public void DealHands()
		{
			Debug.Log( "Dealing hands to the players." );
		}


	}

}
