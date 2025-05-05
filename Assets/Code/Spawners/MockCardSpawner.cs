using DG.Tweening;
using UnityEngine;

namespace Sabacc
{
	/// <summary>
	/// Spawns CardView instances for testing purposes.
	/// </summary>
	public class MockCardSpawner : MonoBehaviour
	{
		[SerializeField]
		private CardView m_CardViewPrefab;

		public static MockCardSpawner Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null )
			{
				Destroy( gameObject );
				return;
			}

			Instance = this;
		}


		public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation, bool showCard = false)
		{
			CardView cardView = Instantiate( m_CardViewPrefab, position, rotation );
			cardView.Setup( card );

			if (showCard == false)
			{
				cardView.ShowBackImmediate();
			}

			// The scaling below is just for fun
			cardView.transform.localScale = Vector3.zero;
			cardView.transform.DOScale( Vector3.one, 0.15f );

			return cardView;
		}
	}
}

