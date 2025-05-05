using UnityEngine;

namespace Sabacc{
	public class MenuManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_MainMenuCanvas;
		[SerializeField]
		private GameObject m_SettingsMenuCanvas;

		private void Start()
		{
			m_MainMenuCanvas.SetActive( false );
			m_SettingsMenuCanvas.SetActive( false );
		}

		private void Update()
		{
			if (InputManager.Instance.MenuOpenCloseInput)
			{
				//  Do Something.
			}
		}
	}
}
