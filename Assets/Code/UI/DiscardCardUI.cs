using System;
using System.Collections.Generic;
using KesselSabacc.Gameplay;
using KesselSabacc.Gameplay.PlayerActions;
using KesselSabacc.Model;
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

			foreach ( DiscardCardAction action in actions )
			{
				GameObject obj = Instantiate( _selectableCardPrefab, _selectableCardsContainer );

				obj.SetActive( true );

				DrawableCardUI selectableCardUI = obj.GetComponent<DrawableCardUI>();

				selectableCardUI.Initialize( action.Card.FrontSprite );

				selectableCardUI.OnClick += () => { onChosen( action ); };

				_selectableCards.Add( obj );
			}
		}
	}
}
