using System.Collections;
using UnityEngine.SceneManagement;

namespace KesselSabacc.Gameplay.GameStates
{
	public class GameOverState : IGameState
	{

		private KesselSabaccGameController _gameController;

		public GameOverState(KesselSabaccGameController gameController)
		{
			_gameController = gameController;
		}

		public IEnumerator OnEnter()
		{
			var winner = _gameController.Model.GetWinner();
			_gameController.uiView.gameOverNotificationUI.SetPlayerName( winner.Name );
			_gameController.uiView.gameOverNotificationUI.Show();
			_gameController.uiView.gameOverNotificationUI.OnContinue += OnContinue;
			yield return null;
		}

		public IEnumerator OnExit()
		{
			_gameController.uiView.gameOverNotificationUI.OnContinue -= OnContinue;
			yield return null;
		}

		public void OnInput()
		{

		}

		public void OnUpdate()
		{

		}

		private void OnContinue()
		{
			SceneManager.LoadScene( "Scenes/MainMenu" );
		}
	}
}
