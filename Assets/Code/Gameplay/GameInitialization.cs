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

			var cpu1 = new Model.Player( "CPU 1" );
			var cpu2 = new Model.Player( "CPU 2" );
			var cpu3 = new Model.Player( "CPU 3" );
			var cpu4 = new Model.Player( "CPU 4" );

			gameController.AddPlayer( cpu1 );
			gameController.AddPlayer( cpu2 );
			gameController.AddPlayer( cpu3 );
			gameController.AddPlayer( cpu4 );

			gameController.AddPlayerController( new SimpleAIController( cpu1 ) );
			gameController.AddPlayerController( new SimpleAIController( cpu2 ) );
			gameController.AddPlayerController( new SimpleAIController( cpu3 ) );
			gameController.AddPlayerController( new SimpleAIController( cpu4 ) );
		}
	}
}
