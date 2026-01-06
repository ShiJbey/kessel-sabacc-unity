using System;
using KesselSabacc.Gameplay.AI;
using UnityEngine;

namespace KesselSabacc.Gameplay
{
	public class GameInitialization : MonoBehaviour
	{
		[SerializeField]
		private GameplayManager _gameplayManager;

		public static event Action OnGameInitialized;

		private void Start()
		{
			InitializeGame();
		}

		private void InitializeGame()
		{
			GameplayManager.Instance.GameController = new KesselSabaccController(
				new Model.KesselSabacc()
			);

			CreateTestGame();

			OnGameInitialized?.Invoke();

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
