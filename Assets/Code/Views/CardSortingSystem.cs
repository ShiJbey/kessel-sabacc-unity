using System.Collections.Generic;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Views
{
	/// <summary>
	/// Manages the visual sorting order of cards in different zones
	/// </summary>
	public class CardSortingSystem : MonoBehaviour
	{
		public static CardSortingSystem Instance { get; private set; }

		[Header( "Sorting Layer Names" )]
		[SerializeField] private string handLayer = "Hand";
		[SerializeField] private string playAreaLayer = "PlayArea";
		[SerializeField] private string discardLayer = "Discard";
		[SerializeField] private string deckLayer = "Deck";

		[Header( "Base Sorting Orders" )]
		[SerializeField] private int handBaseOrder = 100;
		[SerializeField] private int playAreaBaseOrder = 200;
		[SerializeField] private int discardBaseOrder = 50;
		[SerializeField] private int deckBaseOrder = 0;
		[SerializeField] private int draggedCardOrder = 1000;

		private Dictionary<string, int> zoneOrderCounters = new();
		private Dictionary<CardZone, List<CardView>> zoneCards = new();

		private void Awake()
		{
			if ( Instance == null )
			{
				Instance = this;
				foreach ( CardZone zone in System.Enum.GetValues( typeof( CardZone ) ) )
				{
					zoneCards[zone] = new List<CardView>();
				}
			}
			else
			{
				Destroy( gameObject );
			}
		}

		/// <summary>
		/// Sets the sorting for a card when it enters a specific zone
		/// </summary>
		public void SetCardSorting(CardView card, CardZone zone)
		{
			string layerName;
			int baseOrder;

			switch ( zone )
			{
				case CardZone.Hand:
					layerName = handLayer;
					baseOrder = handBaseOrder;
					break;
				case CardZone.PlayArea:
					layerName = playAreaLayer;
					baseOrder = playAreaBaseOrder;
					break;
				case CardZone.Discard:
					layerName = discardLayer;
					baseOrder = discardBaseOrder;
					break;
				case CardZone.Deck:
					layerName = deckLayer;
					baseOrder = deckBaseOrder;
					break;
				default:
					layerName = handLayer;
					baseOrder = handBaseOrder;
					break;
			}

			// Get the next order value for this zone
			if ( !zoneOrderCounters.ContainsKey( zone.ToString() ) )
			{
				zoneOrderCounters[zone.ToString()] = 0;
			}

			int orderInLayer = baseOrder + zoneOrderCounters[zone.ToString()];
			zoneOrderCounters[zone.ToString()]++;

			card.SetSortingLayer( layerName, orderInLayer );
		}

		/// <summary>
		/// Brings a card to the front (useful when dragging)
		/// </summary>
		public void BringToFront(CardView card)
		{
			card.SetSortingOrder( draggedCardOrder );
		}

		/// <summary>
		/// Resets the sorting order counter for a specific zone
		/// </summary>
		public void ResetZoneCounter(CardZone zone)
		{
			zoneOrderCounters[zone.ToString()] = 0;
		}

		/// <summary>
		/// Reorders all cards in a zone (useful after removing a card)
		/// </summary>
		public void ReorderZone(CardZone zone, List<CardView> cards)
		{
			ResetZoneCounter( zone );
			foreach ( CardView card in cards )
			{
				SetCardSorting( card, zone );
			}
		}

		/// <summary>
		/// Adds a card to a zone
		/// </summary>
		public void AddCardToZone(CardView card, CardZone zone)
		{
			// Remove from previous zone
			CardZone previousZone = card.GetCurrentZone();
			if ( zoneCards[previousZone].Contains( card ) )
			{
				zoneCards[previousZone].Remove( card );
			}

			// Add to new zone
			zoneCards[zone].Add( card );
			card.MoveToZone( zone );
		}

		/// <summary>
		/// Gets all cards in a specific zone
		/// </summary>
		public List<CardView> GetCardsInZone(CardZone zone)
		{
			return new List<CardView>( zoneCards[zone] );
		}

		/// <summary>
		/// Removes a card from its current zone
		/// </summary>
		public void RemoveCard(CardView card)
		{
			CardZone zone = card.GetCurrentZone();
			if ( zoneCards[zone].Contains( card ) )
			{
				zoneCards[zone].Remove( card );
				CardSortingSystem.Instance.ReorderZone( zone, zoneCards[zone] );
			}
		}
	}

}
