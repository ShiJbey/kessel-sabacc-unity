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
		/// The spline responsible for dictating hand curviture.
		/// </summary>
		[SerializeField]
		private SplineContainer m_SplineContainer;

		/// <summary>
		/// Does this hand belong to the player playing on this system.
		/// </summary>
		[SerializeField]
		private bool m_IsPlayerHand;

		/// <summary>
		/// All the cards currently in the player's hand.
		/// </summary>
		private List<CardView> m_Cards = new List<CardView>();

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
				m_Cards.Insert( 0, cardView );
			}
			else
			{
				m_Cards.Add( cardView );
			}

			cardView.transform.SetParent( transform );

			if ( m_IsPlayerHand )
			{
				yield return cardView.ShowFrontAsync();
			}

			yield return UpdateCardPositions( 0.15f );
		}


		private IEnumerator UpdateCardPositions(float duration)
		{
			if ( m_Cards.Count == 0 ) yield break;

			// Spline is measured from 0 to 1. Players can have a maximum of
			// 3 cards in their hand at once (drawing during sabacc). We space
			// the cards out evenly over that interval.
			float cardSpacing = 1f / 6f;

			// The first card in the hand defaults to 0.5 (the middle of the spline)
			// We then offset the position to the left by substracting the number of
			// additional cards we have in the hand (handsize - 1) times the amount of
			// spacing between the cards
			float firstCardPosition = 0.5f - (m_Cards.Count - 1) * cardSpacing / 2;

			Vector3 handUp = transform.up;

			Spline spline = m_SplineContainer.Spline;
			for ( int i = 0; i < m_Cards.Count; i++ )
			{
				float p = firstCardPosition + i * cardSpacing;
				Vector3 splinePosition = spline.EvaluatePosition( p );
				Vector3 forward = spline.EvaluateTangent( p );
				Vector3 up = spline.EvaluateUpVector( p );

				Quaternion rotation = Quaternion.LookRotation( up, Vector3.Cross( up, forward ).normalized );

				Vector3 finalRotation = rotation.eulerAngles + transform.rotation.eulerAngles;

				if ( !m_IsPlayerHand )
				{
					finalRotation += new Vector3( 0f, 180f, 0f );
					finalRotation.Scale( new Vector3( 1f, 1f, -1f ) );
				}

				m_Cards[i].transform.DOMove( transform.TransformPoint( splinePosition ) + 0.01f * i * handUp, duration );
				m_Cards[i].transform.DORotate( finalRotation, duration );

				yield return new WaitForSeconds( duration );
			}
		}
	}
}
