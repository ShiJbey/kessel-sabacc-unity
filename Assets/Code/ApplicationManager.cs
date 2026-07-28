using System;
using System.Collections;
using KesselSabacc.UI;
using KesselSabacc.UI.Screens;
using UnityEngine;

namespace KesselSabacc
{
	public class ApplicationManager : MonoBehaviour
	{
		[SerializeField]
		private LoadingScreen _loadingScreenPrefab;
		[SerializeField]
		private SettingsManager _settingsManagerPrefab;
		[SerializeField]
		private AudioManager _audioManagerPrefab;
		[SerializeField]
		private SceneController _sceneControllerPrefab;
		[SerializeField]
		private NewGameManager _newGameManagerPrefab;
		[SerializeField]
		private UIFeedbackManager _uiFeedbackManagerPrefab;

		private Coroutine _initCoroutine = null;

		public LoadingScreen LoadingScreen { get; private set; }

		public bool IsReady { get; private set; } = false;
		public static ApplicationManager Instance { get; private set; }

		public static event Action OnReady;

		private void Awake()
		{
			if ( Instance != null )
			{
				Destroy( gameObject );
				return;
			}

			Instance = this;
		}

		public void Start()
		{
			_initCoroutine = StartCoroutine( InstantiateGameRoutine() );
		}

		private IEnumerator InstantiateGameRoutine()
		{
			LoadingScreen = Instantiate( _loadingScreenPrefab, this.transform ).GetComponent<LoadingScreen>();
			LoadingScreen.Show();
			yield return null;

			Instantiate( _settingsManagerPrefab, this.transform );

			var sceneController = Instantiate( _sceneControllerPrefab, this.transform ).GetComponent<SceneController>();
			sceneController.SetLoadingScreen( LoadingScreen );

			Instantiate( _audioManagerPrefab, this.transform );

			Instantiate( _newGameManagerPrefab, this.transform );

			Instantiate( _uiFeedbackManagerPrefab, this.transform );

			yield return null;

			LoadingScreen.Hide();

			IsReady = true;
			OnReady?.Invoke();

			SceneController.Instance
				.NewTransition()
				.Load( SceneDatabase.Slots.Menu, SceneDatabase.Scenes.MainMenu )
				.WithOverlay()
				.Perform();
		}

		private void OnDestroy()
		{
			if ( _initCoroutine != null )
			{
				StopCoroutine( _initCoroutine );
				_initCoroutine = null;
			}

			if ( Instance == this )
				Instance = null;
		}
	}
}
