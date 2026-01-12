using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KesselSabacc.Gameplay;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Views
{
	/// <summary>
	/// Visual representation of draw decks and discard piles.
	/// </summary>
	public class CardStackView : MonoBehaviour
	{
		[Header( "Animation Settings" )]
		public float deckSpawnDuration = 1f;

		private KesselSabaccGameController _gameController;
		private CardStack _stack;
		private List<CardView> _cards = new();

		public bool IsAnimating { get; private set; } = false;

		public void Initialize(KesselSabaccGameController gameController, CardStack stack)
		{
			_gameController = gameController;
			_stack = stack;
			// model.OnCardAdded += CardStack_OnCardAdded;
			// model.OnCardRemoved += CardStack_OnCardRemoved;
			// model.OnCardsCleared += CardStack_OnCardsCleared;

			// Card topCard = model.Peek();
			// if ( topCard != null )
			// {
			// 	_cardView.Initialize( topCard );
			// 	_cardView.gameObject.SetActive( true );
			// }
			// else
			// {
			// 	_cardView.gameObject.SetActive( false );
			// }
		}

		public CardView Peek()
		{
			if ( _cards.Count > 0 )
			{
				CardView cardView = _cards[_cards.Count - 1];
				return cardView;
			}
			return null;
		}

		public CardView Pop()
		{
			if ( _cards.Count > 0 )
			{
				CardView cardView = _cards[_cards.Count - 1];
				_cards.RemoveAt( _cards.Count - 1 );
				return cardView;
			}
			return null;
		}

		public IEnumerator AnimateDeckSpawn()
		{
			IsAnimating = true;
			int totalCards = _stack.Cards.Count;
			for ( int i = 0; i < totalCards; i++ )
			{
				Card card = _stack.Cards[i];
				CardView cardView = _gameController.uiView.SpawnCard( card, transform.position, transform.rotation );
				_cards.Add( cardView );
				if ( _stack.IsFaceDown )
				{
					cardView.ShowBack();
				}
				else
				{
					cardView.ShowFront();
				}

				// Slight offset for stacking effect
				Vector3 offset = new Vector3( 0, 0.01f * i, -0.01f * i );
				cardView.transform.position = transform.position + offset;

				yield return new WaitForSeconds( deckSpawnDuration / totalCards );
			}
			IsAnimating = false;
		}

		public IEnumerator AddCard(CardView cardView)
		{
			// TODO: Tween the card view from its current position to the stack.
			//       Then replace the current card view with this new one. It
			//       should create the effect of adding a card to the top of the
			//		 stack.
			IsAnimating = true;
			_cards.Add( cardView );

			var sequence = DOTween.Sequence();

			sequence.Append(
				cardView.transform.DOMove( transform.position, 0.35f ).SetEase( Ease.OutQuad )
			);

			sequence.onComplete += () =>
			{
				IsAnimating = false;
			};

			yield return sequence.WaitForCompletion();
		}
	}
}
