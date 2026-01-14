using System;
using System.Collections.Generic;
using KesselSabacc.Model;
using KesselSabacc.UI.Components;
using KesselSabacc.Views;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI
{
	public class DiscardCardUI : UIComponent
	{
		[Header( "References" )]
		[SerializeField]
		private GameObject _selectableCardPrefab;
		[SerializeField]
		private RectTransform _selectableCardsContainer;
		[SerializeField]
		private Image _otherCard;

		private List<GameObject> _selectableCards = new();

		public event Action<Card> OnCardSelected;

		public void UpdateView(Card[] disposableCards, Card otherCard)
		{
			foreach ( GameObject cardUI in _selectableCards )
			{
				Destroy( cardUI );
			}
			_selectableCards.Clear();

			foreach ( Card card in disposableCards )
			{
				GameObject obj = Instantiate( _selectableCardPrefab, _selectableCardsContainer );

				obj.SetActive( true );

				DrawableCardUI selectableCardUI = obj.GetComponent<DrawableCardUI>();

				selectableCardUI.Initialize(
					KesselSabaccGameView.Instance.GetCardFront( card.Suit, card.CardType ) );

				selectableCardUI.OnClick += () => HandleCardSelected( card );

				_selectableCards.Add( obj );
			}

			_otherCard.sprite = KesselSabaccGameView.Instance.GetCardFront(
				otherCard.Suit, otherCard.CardType );
		}

		public void HandleCardSelected(Card card)
		{
			OnCardSelected?.Invoke( card );
		}
	}
}
