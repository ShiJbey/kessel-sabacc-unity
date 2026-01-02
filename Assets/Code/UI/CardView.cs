using System.Collections;
using DG.Tweening;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.UI
{
	/// <summary>
	/// Manages the presentation of a single card on the screen.
	/// </summary>
	public class CardView : MonoBehaviour
	{
		/// <summary>
		/// The SpriteRenderer for the front of the card.
		/// </summary>
		[SerializeField]
		private SpriteRenderer m_FrontSpriteRenderer;

		/// <summary>
		/// The SpriteRenderer for the back of the card.
		/// </summary>
		[SerializeField]
		private SpriteRenderer m_BackSpriteRenderer;

		/// <summary>
		/// The card this view visualizes.
		/// </summary>
		public Card Card { get; private set; }


		/// <summary>
		/// Initialize the card appearance using a Card Object
		/// </summary>
		/// <param name="card"></param>
		public void Initialize(Card card, Sprite frontSprite, Sprite backSprite)
		{
			Card = card;
			SetFrontSprite( frontSprite );
			SetBackSprite( backSprite );
		}

		/// <summary>
		/// Show the front of the card using a flip animation.
		/// </summary>
		/// <param name="duration"></param>
		/// <returns></returns>
		public IEnumerator ShowFront(float duration = 0.15f)
		{
			transform.DOLocalRotate( new Vector3( 0, 0, 0 ), duration );
			yield return new WaitForSeconds( duration );
		}

		/// <summary>
		/// Show the back of the card using a flip animation.
		/// </summary>
		/// <param name="duration"></param>
		/// <returns></returns>
		public IEnumerator ShowBack(float duration = 0.15f)
		{
			transform.DOLocalRotate( new Vector3( 0, 180, 0 ), duration );
			yield return new WaitForSeconds( duration );
		}

		/// <summary>
		/// Show the front of the card without any flip animation.
		/// </summary>
		public void ShowFrontImmediate()
		{
			transform.localRotation = Quaternion.Euler( new Vector3( 0, 0, 0 ) );
		}

		/// <summary>
		/// Show the back of the card without any flip animation.
		/// </summary>
		public void ShowBackImmediate()
		{
			transform.localRotation = Quaternion.Euler( new Vector3( 0, 180, 0 ) );
		}

		/// <summary>
		/// Set the sprite for the front of the card.
		/// </summary>
		/// <param name="sprite"></param>
		public void SetFrontSprite(Sprite sprite)
		{
			m_FrontSpriteRenderer.sprite = sprite;
		}

		/// <summary>
		/// Set the sprite for the back of the card.
		/// </summary>
		/// <param name="sprite"></param>
		public void SetBackSprite(Sprite sprite)
		{
			m_BackSpriteRenderer.sprite = sprite;
		}
	}

}
