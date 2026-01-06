using UnityEngine;

namespace KesselSabacc.UI
{
	/// <summary>
	/// Manages references to commonly-used sounds within the user interface.
	/// </summary>
	public class UIFeedbackManager : MonoBehaviour
	{
		[SerializeField]
		private AudioClip _buttonClickSound;

		public static UIFeedbackManager Instance { get; private set; }

		private void Awake()
		{
			if ( Instance != null )
			{
				Destroy( gameObject );
				return;
			}

			Instance = this;
		}

		public void PlayButtonClickSound()
		{
			if ( _buttonClickSound != null )
			{
				AudioManager.PlayOneSFX( _buttonClickSound, Vector3.zero );
			}
		}
	}
}
