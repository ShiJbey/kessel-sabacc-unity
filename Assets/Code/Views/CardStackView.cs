using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Views
{
	/// <summary>
	/// Visual representation of draw decks and discard piles.
	/// </summary>
	public class CardStackView : MonoBehaviour
	{
		[SerializeField]
		private CardView _cardView;

		private CardStack _model;

		public void Initialize(CardStack model)
		{
			_model = model;
			model.OnCardAdded += CardStack_OnCardAdded;
			model.OnCardRemoved += CardStack_OnCardRemoved;
			model.OnCardsCleared += CardStack_OnCardsCleared;

			Card topCard = model.Peek();
			if ( topCard != null )
			{
				_cardView.Initialize( topCard );
				_cardView.gameObject.SetActive( true );
			}
			else
			{
				_cardView.gameObject.SetActive( false );
			}
		}

		private void CardStack_OnCardAdded(Card card)
		{
			// TODO: Animate card moving to this stack
			Card topCard = _model.Peek();
			if ( topCard != null )
			{
				_cardView.Initialize( topCard );
				_cardView.gameObject.SetActive( true );
			}
			else
			{
				_cardView.gameObject.SetActive( false );
			}
		}

		private void CardStack_OnCardRemoved(Card card)
		{
			// TODO: Animate care leaving the stack
			Card topCard = _model.Peek();
			if ( topCard != null )
			{
				_cardView.Initialize( topCard );
				_cardView.gameObject.SetActive( true );
			}
			else
			{
				_cardView.gameObject.SetActive( false );
			}
		}

		private void CardStack_OnCardsCleared()
		{
			// TODO: Play animation of the cards disappearing
			Card topCard = _model.Peek();
			if ( topCard != null )
			{
				_cardView.Initialize( topCard );
				_cardView.gameObject.SetActive( true );
			}
			else
			{
				_cardView.gameObject.SetActive( false );
			}
		}

		public void AddCardAnimation(CardView cardView)
		{
			// TODO: Tween the card view from its current position to the stack.
			//       Then replace the current card view with this new one. It
			//       should create the effect of adding a card to the top of the
			//		 stack.
		}
	}
}
