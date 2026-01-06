using KesselSabacc.Gameplay;
using KesselSabacc.Model;
using KesselSabacc.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI.Components
{
	public class TableView : UIComponent
	{
		[SerializeField]
		private Image _sandDiscardPileImage;
		[SerializeField]
		private Image _sandDeckImage;
		[SerializeField]
		private Image _bloodDeckImage;
		[SerializeField]
		private Image _bloodDiscardPileImage;

		public void Initialize(Model.KesselSabacc game)
		{
			game.SandDeck.OnCardAdded += HandleSandDeckChanged;
			game.SandDeck.OnCardRemoved += HandleSandDeckChanged;
			game.SandDiscardPile.OnCardAdded += HandleSandDiscardPileChanged;
			game.SandDiscardPile.OnCardRemoved += HandleSandDiscardPileChanged;
			game.BloodDeck.OnCardAdded += HandleBloodDeckChanged;
			game.BloodDeck.OnCardRemoved += HandleBloodDeckChanged;
			game.BloodDiscardPile.OnCardAdded += HandleBloodDiscardPileChanged;
			game.BloodDiscardPile.OnCardRemoved += HandleBloodDiscardPileChanged;

			UpdateSandDiscardPile();
			UpdateSandDeck();
			UpdateBloodDeck();
			UpdateBloodDiscardPile();
		}

		public void UpdateSandDiscardPile()
		{
			var game = GameplayManager.Instance.GameController.Model;
			UpdateCardPileImage( _sandDiscardPileImage, game.SandDiscardPile.Peek(), false );
		}

		public void UpdateSandDeck()
		{
			var game = GameplayManager.Instance.GameController.Model;
			UpdateCardPileImage( _sandDeckImage, game.SandDeck.Peek(), true );
		}

		public void UpdateBloodDeck()
		{
			var game = GameplayManager.Instance.GameController.Model;
			UpdateCardPileImage( _bloodDeckImage, game.BloodDeck.Peek(), true );
		}

		public void UpdateBloodDiscardPile()
		{
			var game = GameplayManager.Instance.GameController.Model;
			UpdateCardPileImage( _bloodDiscardPileImage, game.BloodDiscardPile.Peek(), false );
		}

		private void HandleSandDiscardPileChanged(Card card)
		{
			UpdateSandDiscardPile();
		}

		private void HandleBloodDiscardPileChanged(Card card)
		{
			UpdateBloodDiscardPile();
		}

		private void HandleSandDeckChanged(Card card)
		{
			UpdateSandDeck();
		}

		private void HandleBloodDeckChanged(Card card)
		{
			UpdateBloodDeck();
		}


		private void UpdateCardPileImage(Image image, Card card, bool showCardBack)
		{
			if ( card != null )
			{
				image.sprite = showCardBack ?
					GameplayManager.Instance.GetCardBack( card.Suit )
					: GameplayManager.Instance.GetCardFront( card.Suit, card.CardType );
				image.color = Color.white;
			}
			else
			{
				image.color = Color.white.WithAlpha( 0 );
			}
		}
	}
}
