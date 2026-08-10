using System;
using System.Collections.Generic;
using KesselSabacc.Model;
using KesselSabacc.Model.PlayerActions;
using KesselSabacc.UI.Components;
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

		public void UpdateView(IReadOnlyList<DiscardCardAction> actions, Action<PlayerAction> onChosen)
		{
			foreach ( GameObject cardUI in _selectableCards )
			{
				Destroy( cardUI );
			}
			_selectableCards.Clear();

			DeckConfiguration deckConfig = NewGameManager.Instance.Data.deck;

			foreach ( DiscardCardAction action in actions )
			{
				GameObject obj = Instantiate( _selectableCardPrefab, _selectableCardsContainer );

				obj.SetActive( true );

				DrawableCardUI selectableCardUI = obj.GetComponent<DrawableCardUI>();

				selectableCardUI.Initialize( deckConfig.GetFrontSprite(action.cardSuit, action.cardType) );

				selectableCardUI.OnClick += () => { onChosen( action ); };

				_selectableCards.Add( obj );
			}
		}
	}
}
