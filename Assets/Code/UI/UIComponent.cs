using UnityEngine;

namespace KesselSabacc.UI
{
	public abstract class UIComponent : MonoBehaviour
	{
		[Header( "Visibility" )]
		[SerializeField] private bool m_HideOnAwake;

		private CanvasGroup m_CanvasGroup;

		protected virtual void Awake()
		{
			m_CanvasGroup = GetComponent<CanvasGroup>();

			if ( m_HideOnAwake )
			{
				Hide();
			}

			SubscribeToEvents();
		}

		protected virtual void OnDestroy()
		{
			UnsubscribeFromEvents();
		}

		protected virtual void SubscribeToEvents()
		{
			// Do Nothing
		}

		protected virtual void UnsubscribeFromEvents()
		{
			// Do Nothing
		}

		public virtual void Show()
		{
			if ( m_CanvasGroup )
			{
				m_CanvasGroup.alpha = 1;
				m_CanvasGroup.interactable = true;
				m_CanvasGroup.blocksRaycasts = true;
			}
			else
			{
				gameObject.SetActive( true );
			}
		}

		public virtual void Hide()
		{
			if ( m_CanvasGroup )
			{
				m_CanvasGroup.alpha = 0;
				m_CanvasGroup.interactable = false;
				m_CanvasGroup.blocksRaycasts = false;
			}
			else
			{
				gameObject.SetActive( false );
			}
		}
	}
}
