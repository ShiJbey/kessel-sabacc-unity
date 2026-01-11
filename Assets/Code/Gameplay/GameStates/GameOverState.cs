using System.Collections;
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

		public IEnumerator OnEnter()
		{
			yield return null;
		}

		public IEnumerator OnExit()
		{
			yield return null;
		}

		public void OnInput()
		{

		}

		public void OnUpdate()
		{

		}
	}
}
