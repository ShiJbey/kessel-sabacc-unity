using System.Threading.Tasks;

namespace KesselSabacc.Gameplay.GameStates
{
	public class GameOverState : IGameState
	{
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
