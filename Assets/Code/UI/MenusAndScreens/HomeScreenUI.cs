using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KesselSabacc
{
    /// <summary>
    /// Manages the presentation of the HomeScreen.
    /// </summary>
    public class HomeScreenUI : MonoBehaviour
    {
        [SerializeField]
        private Button m_PlayButton;

        [SerializeField]
        private Button m_ExitButton;


        void Start()
        {
            EventSystem.current.firstSelectedGameObject = m_PlayButton.gameObject;

#if UNITY_WEBGL
            m_ExitButton.gameObject.SetActive( false );
#endif
        }


        private void OnEnable()
        {
            m_PlayButton.onClick.AddListener( HandlePlayButtonClicked );
            m_ExitButton.onClick.AddListener( HandleExitButtonClicked );
        }

        private void OnDisable()
        {
            m_PlayButton.onClick.RemoveListener( HandlePlayButtonClicked );
            m_ExitButton.onClick.RemoveListener( HandleExitButtonClicked );
        }

        private void HandlePlayButtonClicked()
        {
            // Loads the main menu scene
            SceneManager.LoadScene( 1 );
        }

        private void HandleExitButtonClicked()
        {
#if UNITY_STANDALONE
			Application.Quit();
#endif

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
