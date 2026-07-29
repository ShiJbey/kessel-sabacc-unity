using System;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc
{
	[CreateAssetMenu( fileName = "DeckConfiguration", menuName = "Sabacc/DeckConfiguration" )]
	public class DeckConfiguration : ScriptableObject
	{
		public Sprite bloodCardBack;
		public Sprite sandCardBack;
		public DeckCardConfig[] cards;
	}

	[Serializable]
	public class DeckCardConfig
	{
		public Sprite bloodFront;
		public Sprite sandFront;
		public CardType cardType;
		public int count;
	}
}
