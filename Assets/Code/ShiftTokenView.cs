using UnityEngine;

namespace Sabacc
{
	/// <summary>
	///  Manages the presentation of a single shift token on the screen.
	/// </summary>
	public class ShiftTokenView : MonoBehaviour
	{
		/// <summary>
		/// The SpriteRenderer for the front of the shift token.
		/// </summary>
		[SerializeField]
		private SpriteRenderer m_FrontSpriteRenderer;

		/// <summary>
		/// The SpriteRenderer for the back of the shift token.
		/// </summary>
		[SerializeField]
		private SpriteRenderer m_BackSpriteRenderer;

		/// <summary>
		/// Set the sprite for the front of the shift token.
		/// </summary>
		/// <param name="sprite"></param>
		public void SetFrontSprite(Sprite sprite)
		{
			m_FrontSpriteRenderer.sprite = sprite;
		}

		/// <summary>
		/// Set the sprite for the back of the shift token.
		/// </summary>
		/// <param name="sprite"></param>
		public void SetBackSprite(Sprite sprite)
		{
			m_BackSpriteRenderer.sprite = sprite;
		}
	}
}


