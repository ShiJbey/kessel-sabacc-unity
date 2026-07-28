using System.Collections;
using System.Collections.Generic;
using KesselSabacc.UI.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KesselSabacc
{
	public class SceneController : MonoBehaviour
	{
		[SerializeField]
		private LoadingScreen _loadingScreen;

		private Dictionary<string, string> _loadedSceneBySlot = new();
		private bool _isBusy;

		public static SceneController Instance { get; private set; }

		private void Awake()
		{
			if ( Instance != null )
			{
				Destroy( gameObject );
				return;
			}

			Instance = this;
		}

		private void OnDestroy()
		{
			if ( Instance == this ) Instance = null;
		}

		public void SetLoadingScreen(LoadingScreen screen)
		{
			_loadingScreen = screen;
		}

		public Coroutine ExecutePlan(SceneTransitionPlan plan)
		{
			if ( _isBusy )
			{
				Debug.LogWarning( "Scene transition already in progress" );
				return null;
			}

			_isBusy = true;
			return StartCoroutine( TransitionSceneRoutine( plan ) );
		}

		private IEnumerator TransitionSceneRoutine(SceneTransitionPlan plan)
		{
			if ( plan.Overlay )
			{
				_loadingScreen.SetProgress( 0f );
				_loadingScreen.Show();
				yield return new WaitForSeconds( 1f );
			}

			foreach ( string slotKey in plan.ScenesToUnload )
			{
				yield return UnloadSceneRoutine( slotKey );
			}

			if ( plan.ClearUnusedAssets )
			{
				yield return CleanupUnusedAssetsRoutine();
			}

			foreach ( var pair in plan.ScenesToLoad )
			{
				if ( _loadedSceneBySlot.ContainsKey( pair.Key ) )
				{
					yield return UnloadSceneRoutine( pair.Key );
				}

				yield return LoadAdditiveRoutine( pair.Key, pair.Value, plan.ActiveSceneName == pair.Value );
			}

			if ( plan.Overlay )
			{
				_loadingScreen.Hide();
			}

			_isBusy = false;
		}

		private IEnumerator LoadAdditiveRoutine(string slotKey, string sceneName, bool setActive)
		{
			AsyncOperation loadOp = SceneManager.LoadSceneAsync( sceneName, LoadSceneMode.Additive );
			if ( loadOp == null ) yield break;

			loadOp.allowSceneActivation = false;

			while ( loadOp.progress < 0.9f )
			{
				_loadingScreen.SetProgress( loadOp.progress );
				yield return new WaitForSeconds( 0.1f );
				yield return null;
			}

			loadOp.allowSceneActivation = true;
			while ( !loadOp.isDone )
			{
				yield return null;
			}

			_loadingScreen.SetProgress( 1f );

			if ( setActive )
			{
				var scene = SceneManager.GetSceneByName( sceneName );
				if ( scene.IsValid() && scene.isLoaded )
				{
					SceneManager.SetActiveScene( scene );
				}
			}

			_loadedSceneBySlot[slotKey] = sceneName;
		}

		private IEnumerator UnloadSceneRoutine(string slotKey)
		{
			if ( !_loadedSceneBySlot.TryGetValue( slotKey, out string sceneName ) )
			{
				yield break;
			}

			if ( string.IsNullOrEmpty( sceneName ) )
			{
				yield break;
			}

			AsyncOperation unloadOp = SceneManager.UnloadSceneAsync( sceneName );

			if ( unloadOp != null )
			{
				while ( !unloadOp.isDone )
				{
					yield return null;
				}
			}

			_loadedSceneBySlot.Remove( slotKey );
		}

		private IEnumerator CleanupUnusedAssetsRoutine()
		{
			AsyncOperation cleanupOp = Resources.UnloadUnusedAssets();
			while ( !cleanupOp.isDone )
			{
				yield return null;
			}
		}

		public SceneTransitionPlan NewTransition()
		{
			return new SceneTransitionPlan();
		}

		public class SceneTransitionPlan
		{
			public Dictionary<string, string> ScenesToLoad { get; } = new();
			public List<string> ScenesToUnload { get; } = new();
			public string ActiveSceneName { get; private set; } = "";
			public bool ClearUnusedAssets { get; private set; } = false;
			public bool Overlay { get; private set; } = false;

			public SceneTransitionPlan Load(string slotKey, string sceneName, bool setActive = false)
			{
				ScenesToLoad[slotKey] = sceneName;
				ActiveSceneName = setActive ? sceneName : "";
				return this;
			}

			public SceneTransitionPlan Unload(string slotKey)
			{
				ScenesToUnload.Add( slotKey );
				return this;
			}

			public SceneTransitionPlan WithOverlay()
			{
				Overlay = true;
				return this;
			}

			public SceneTransitionPlan WithClearUnusedAssets()
			{
				ClearUnusedAssets = true;
				return this;
			}

			public Coroutine Perform()
			{
				return SceneController.Instance.ExecutePlan( this );
			}
		}
	}
}
