using System.Threading.Tasks;

namespace KesselSabacc.Gameplay
{
	public interface IGameState
	{
		/// <summary>
		/// Called when entering this state
		/// </summary>
		public Task OnEnter();

		/// <summary>
		/// Called when exiting this state
		/// </summary>
		public Task OnExit();

		/// <summary>
		/// Handle updates.
		/// </summary>
		void OnUpdate();

		/// <summary>
		/// Handle input
		/// </summary>
		void OnInput();
	}
}
