using KesselSabacc.Gameplay;
using KesselSabacc.Model;
using KesselSabacc.Utils;
using UnityEngine;

namespace KesselSabacc.UI.Components
{
	public class TableView : UIComponent
	{
		[SerializeField]
		private CardView _sandDiscardPileImage;
		[SerializeField]
		private CardView _sandDeckImage;
		[SerializeField]
		private CardView _bloodDeckImage;
		[SerializeField]
		private CardView _bloodDiscardPileImage;

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
			UpdateCardPileImage( _sandDiscardPileImage, game.SandDiscardPile.Peek() );
		}

		public void UpdateSandDeck()
		{
			var game = GameplayManager.Instance.GameController.Model;
			UpdateCardPileImage( _sandDeckImage, game.SandDeck.Peek() );
		}

		public void UpdateBloodDeck()
		{
			var game = GameplayManager.Instance.GameController.Model;
			UpdateCardPileImage( _bloodDeckImage, game.BloodDeck.Peek() );
		}

		public void UpdateBloodDiscardPile()
		{
			var game = GameplayManager.Instance.GameController.Model;
			UpdateCardPileImage( _bloodDiscardPileImage, game.BloodDiscardPile.Peek() );
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
			// Spawn a new card view using the card
			// Fade it in and tween it to the deck position.

			UpdateSandDeck();
		}

		private void HandleBloodDeckChanged(Card card)
		{
			UpdateBloodDeck();
		}


		private void UpdateCardPileImage(CardView view, Card card)
		{
			// if ( card != null )
			// {
			// 	view.Initialize( card );
			// }
			// else
			// {
			// 	image.color = Color.white.WithAlpha( 0 );
			// }
		}
	}
}
