using System;
using System.Collections;
using KesselSabacc.Gameplay.AI;
using KesselSabacc.UI;
using KesselSabacc.UI.Screens;
using KesselSabacc.Views;
using UnityEngine;

namespace KesselSabacc.Gameplay
{
	public class GameInitialization : MonoBehaviour
	{
		[SerializeField]
		private GameplayManager _gameplayManager;
		[SerializeField]
		private DeckConfiguration _deckConfig;
		[SerializeField]
		private TableView _tableView;

		public static event Action OnGameInitialized;

		private void Start()
		{
			StartCoroutine( InitializeGame() );
		}

		private IEnumerator InitializeGame()
		{
			yield return new WaitUntil( () => AutoLoadManager.Instance.isReady );

			var loadingScreen = FindFirstObjectByType<LoadingScreen>( FindObjectsInactive.Include );
			loadingScreen.Show();

			yield return null;

			GameplayManager.Instance.GameController = new KesselSabaccController(
				new Model.KesselSabacc(),
				_deckConfig
			);

			CreateTestGame();
			yield return null;

			_tableView.Initialize( GameplayManager.Instance.GameController.Model );
			GameUIManager.Instance.InitializeUI();
			yield return null;

			OnGameInitialized?.Invoke();
			loadingScreen.Hide();
			yield return null;

			_gameplayManager.StartGame();
		}

		private void CreateTestGame()
		{
			var gameController = GameplayManager.Instance.GameController;

			if ( NewGameManager.Instance.Data == null )
			{
				NewGameManager.Instance.CreateNewGame();
			}

			NewGameData newGameData = NewGameManager.Instance.Data;

			Debug.Log(
				$"Creating a new game with {newGameData.numPlayers} players and {newGameData.numChips} chips"
			);

			// Add human player
			var player = new Model.Player( "Player 1" );
			player.Chips = newGameData.numChips;
			gameController.AddPlayer( player );

			// Add CPU player(s)
			for ( int i = 1; i <= newGameData.numPlayers - 1; i++ )
			{
				var cpu = new Model.Player( $"CPU {i}" );
				cpu.Chips = newGameData.numChips;
				gameController.AddPlayer( cpu );
				gameController.AddPlayerController( new SimpleAIController( cpu ) );
			}
		}
	}
}
