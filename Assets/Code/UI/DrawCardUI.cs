using System;
using System.Collections.Generic;
using KesselSabacc.Gameplay;
using KesselSabacc.Model;
using KesselSabacc.UI.Components;
using KesselSabacc.Views;
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

		public event Action<int> OnCardDrawn;

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

		public void UpdateView(KesselSabaccGameView gameView)
		{
			foreach ( var view in _cardViews )
			{
				Destroy( view.gameObject );
			}
			_cardViews.Clear();

			var cardView = InstantiateCardView( gameView.tableView.SandDiscardPileView.Peek() );
			cardView.OnClick += () => { OnCardDrawn?.Invoke( 0 ); };
			_cardViews.Add( cardView );


			cardView = InstantiateCardView( gameView.tableView.SandDeckView.Peek() );
			cardView.OnClick += () => { OnCardDrawn?.Invoke( 1 ); };
			_cardViews.Add( cardView );

			cardView = InstantiateCardView( gameView.tableView.BloodDeckView.Peek() );
			cardView.OnClick += () => { OnCardDrawn?.Invoke( 2 ); };
			_cardViews.Add( cardView );

			cardView = InstantiateCardView( gameView.tableView.BloodDiscardPileView.Peek() );
			cardView.OnClick += () => { OnCardDrawn?.Invoke( 3 ); };
			_cardViews.Add( cardView );
		}

		public DrawableCardUI InstantiateCardView(CardView cardView)
		{
			var drawableCardView = Instantiate( _cardPrefab, _drawableCardContainer );
			drawableCardView.gameObject.SetActive( true );
			drawableCardView.Initialize( cardView.Sprite );
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
