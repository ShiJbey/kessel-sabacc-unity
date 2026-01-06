using System.Collections.Generic;
using KesselSabacc.Gameplay;
using KesselSabacc.Model;
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

		public override void Show()
		{
			UpdateView( GameplayManager.Instance.GameController.Model );
			base.Show();
		}

		public void UpdateView(Model.KesselSabacc game)
		{
			foreach ( var view in _cardViews )
			{
				Destroy( view.gameObject );
			}
			_cardViews.Clear();

			var cardView = InstantiateCardView( game.SandDiscardPile.Peek(), false );
			_cardViews.Add( cardView );

			cardView = InstantiateCardView( game.SandDeck.Peek(), true );
			_cardViews.Add( cardView );

			cardView = InstantiateCardView( game.BloodDeck.Peek(), true );
			_cardViews.Add( cardView );

			cardView = InstantiateCardView( game.BloodDiscardPile.Peek(), false );
			_cardViews.Add( cardView );
		}

		public DrawableCardUI InstantiateCardView(Card card, bool isFaceDown)
		{
			var cardView = Instantiate( _cardPrefab, _drawableCardContainer );
			cardView.gameObject.SetActive( true );

			if ( isFaceDown )
			{
				cardView.SetCardSprite( GameplayManager.Instance.GetCardBack( card.Suit ) );
			}
			else
			{
				cardView.SetCardSprite( GameplayManager.Instance.GetCardFront( card.Suit, card.CardType ) );
			}

			return cardView;
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
