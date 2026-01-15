using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KesselSabacc.Model;
using UnityEngine;
using UnityEngine.Splines;

namespace KesselSabacc.Views
{
	/// <summary>
	/// Manages the presentation of the player's hand on the screen.
	/// </summary>
	public class HandView : MonoBehaviour
	{
		/// <summary>
		/// The spline responsible for dictating hand curvature.
		/// </summary>
		[SerializeField]
		private SplineContainer _splineContainer;

		/// <summary>
		/// Time it takes for cards to reposition in a player's hand.
		/// </summary>
		[SerializeField]
		private float _cardRepositionAnimTime = 0.15f;

		/// <summary>
		/// All the cards currently in the player's hand.
		/// </summary>
		[SerializeField]
		private List<CardView> _cards = new();

		public IReadOnlyList<CardView> Cards => _cards;

		private IEnumerator Start()
		{
			yield return UpdateCardPositions();
		}

		/// <summary>
		/// Adds a card to the player's hand.
		/// </summary>
		/// <param name="cardView"></param>
		/// <returns></returns>
		public IEnumerator AddCard(CardView cardView)
		{
			if ( cardView.Card.Suit == CardSuit.SAND )
			{
				// This card need to be appended to the front of the list
				_cards.Insert( 0, cardView );
			}
			else
			{
				_cards.Add( cardView );
			}

			cardView.transform.SetParent( transform );

			yield return UpdateCardPositions();
		}

		public CardView GetCard(Card card)
		{
			foreach ( CardView view in _cards )
			{
				if ( view.Card == card )
				{
					return view;
				}
			}
			return null;
		}

		public IEnumerator RemoveCard(Card card)
		{
			for ( int i = _cards.Count - 1; i >= 0; i-- )
			{
				if ( _cards[i].Card == card )
				{
					_cards.RemoveAt( i );
				}
			}
			yield return UpdateCardPositions();
		}

		public void Clear()
		{
			foreach ( CardView cardView in _cards )
			{
				Destroy( cardView.gameObject );
			}
			_cards.Clear();
		}

		private IEnumerator UpdateCardPositions()
		{
			if ( _cards.Count == 0 ) yield break;

			// Spline is measured from 0 to 1. Players can have a maximum of
			// 3 cards in their hand at once (drawing during sabacc). We space
			// the cards out evenly over that interval.
			float cardSpacing = 1f / 6f;

			// The first card in the hand defaults to 0.5 (the middle of the spline)
			// We then offset the position to the left by substracting the number of
			// additional cards we have in the hand (handsize - 1) times the amount of
			// spacing between the cards
			float firstCardPosition = 0.5f - (_cards.Count - 1) * cardSpacing / 2;

			Vector3 handUp = transform.up;
			Spline spline = _splineContainer.Spline;
			var sequence = DOTween.Sequence();

			for ( int i = 0; i < _cards.Count; i++ )
			{
				float p = firstCardPosition + i * cardSpacing;
				Vector3 splinePosition = spline.EvaluatePosition( p );
				Vector3 forward = spline.EvaluateTangent( p );
				Vector3 up = spline.EvaluateUpVector( p );

				Quaternion rotation = Quaternion.LookRotation( up, Vector3.Cross( up, forward ).normalized );

				Vector3 finalRotation = rotation.eulerAngles + transform.rotation.eulerAngles;

				sequence.Join(
					_cards[i].transform.DOMove(
						transform.TransformPoint( splinePosition ) + 0.01f * i * handUp, _cardRepositionAnimTime
					)
				);
				sequence.Join( _cards[i].transform.DORotate( finalRotation, _cardRepositionAnimTime ) );
			}

			yield return sequence.WaitForCompletion();
		}
	}
}
