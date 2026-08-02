using System;
using System.Collections.Generic;
using KesselSabacc.Gameplay;
using KesselSabacc.Gameplay.PlayerActions;
using KesselSabacc.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI
{
	/// <summary>
	/// UI Displayed when drawing a card.
	/// </summary>
	public class DrawCardUI : UIComponent
	{
		[SerializeField]
		private DrawableCardUI _cardPrefab;
		[SerializeField]
		private RectTransform _drawableCardContainer;
		[SerializeField]
		private TMP_Text _descriptionText;
		[SerializeField]
		private Button _backButton;

		private List<DrawableCardUI> _cardViews = new();

		private void Start()
		{
			_cardPrefab.gameObject.SetActive( false );
		}

		protected override void SubscribeToEvents()
		{
			_backButton.onClick.AddListener( OnBackButtonClicked );
		}

		protected override void UnsubscribeFromEvents()
		{
			_backButton.onClick.RemoveListener( OnBackButtonClicked );
		}

		public void UpdateView(IReadOnlyList<DrawCardAction> actions, Action<PlayerAction> onChosen)
		{
			foreach ( var view in _cardViews )
			{
				Destroy( view.gameObject );
			}
			_cardViews.Clear();

			var cardView = InstantiateCardView( actions[0].Card.FrontSprite );
			cardView.OnClick += () => { onChosen( actions[0] ); };
			_cardViews.Add( cardView );

			cardView = InstantiateCardView( actions[1].Card.BackSprite );
			cardView.OnClick += () => { onChosen( actions[1] ); };
			_cardViews.Add( cardView );

			cardView = InstantiateCardView( actions[2].Card.BackSprite );
			cardView.OnClick += () => { onChosen( actions[2] ); };
			_cardViews.Add( cardView );

			cardView = InstantiateCardView( actions[3].Card.FrontSprite );
			cardView.OnClick += () => { onChosen( actions[3] ); };
			_cardViews.Add( cardView );
		}

		public DrawableCardUI InstantiateCardView(Sprite cardSprite)
		{
			var drawableCardView = Instantiate( _cardPrefab, _drawableCardContainer );
			drawableCardView.gameObject.SetActive( true );
			drawableCardView.Initialize( cardSprite );
			return drawableCardView;
		}

		public void SetDeckName(string value)
		{

		}

		private void OnBackButtonClicked()
		{
			Hide();
		}
	}
}
