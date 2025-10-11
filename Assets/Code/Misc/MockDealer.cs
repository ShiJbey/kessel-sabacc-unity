using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sabacc
{
	/// <summary>
	/// Tests dealing cards to all the player hands.
	/// </summary>
	public class MockDealer : MonoBehaviour
	{
		/// <summary>
		/// Hands to deal to.
		/// </summary>
		[SerializeField]
		private List<HandView> m_Hands;

		[SerializeField]
		private DeckView m_SandDeck;

		[SerializeField]
		private DeckView m_BloodDeck;


		private bool m_AreHandsDelt = false;

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Tab) && !m_AreHandsDelt)
			{
				StartCoroutine( DealHands() );
			}
		}

		private IEnumerator DealHands()
		{
			m_AreHandsDelt = true;

			foreach (HandView hand in m_Hands)
			{
				CardView sandCardView = MockCardSpawner.Instance.CreateCardView(
					m_SandDeck.DrawCard(), m_SandDeck.transform.position, Quaternion.identity, false );

				yield return hand.AddCard( sandCardView );

				CardView bloodCardView = MockCardSpawner.Instance.CreateCardView(
					m_BloodDeck.DrawCard(), m_BloodDeck.transform.position, Quaternion.identity, false );

				yield return hand.AddCard( bloodCardView );
			}
		}


	}
}

