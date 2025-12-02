using Sabacc;
using UnityEngine;

public class CardSpawnerTester : MonoBehaviour
{
	[SerializeField]
	private HandView m_HandView;

	[SerializeField]
	private CardData m_CardData;

	[SerializeField]
	private Transform m_DeckTransform;

	private void Update()
	{
		if ( Input.GetKeyUp( KeyCode.Space ))
		{
			CardView cardView = MockCardSpawner.Instance.CreateCardView(new Card(m_CardData), m_DeckTransform.position, Quaternion.identity, false );

			StartCoroutine( m_HandView.AddCard( cardView ) );
		}
	}
}
