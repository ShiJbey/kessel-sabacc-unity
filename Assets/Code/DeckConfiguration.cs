using System.Collections.Generic;
using System;
using UnityEngine;

namespace Sabacc
{
	[CreateAssetMenu( fileName = "DeckConfiguration", menuName = "Sabacc/DeckConfiguration" )]
	public class DeckConfiguration : ScriptableObject
	{
		[Tooltip("The types of cards add to a fresh version of this deck.")]
		[SerializeField]
		public List<DeckCardCount> m_CardCounts;

		public IEnumerable<DeckCardCount> CardCounts => m_CardCounts;

	}

	[Serializable]
	public class DeckCardCount
	{
		[Tooltip("The card type.")]
		public CardData cardData;

		[Tooltip("The number of this card that should be in a fresh deck.")]
		public int count;
	}
}

