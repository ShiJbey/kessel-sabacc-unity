using System.Threading.Tasks;

namespace KesselSabacc.Gameplay.GameStates
{
	public class GameOverState : IGameState
	{

		private KesselSabaccGameController _gameController;

		public GameOverState(KesselSabaccGameController gameController)
		{
			_gameController = gameController;
		}

		public Task OnEnter()
		{
			return Task.CompletedTask;
		}

		public Task OnExit()
		{
			return Task.CompletedTask;
		}

		public void OnInput()
		{

		}

		public void OnUpdate()
		{

		}
	}
}
