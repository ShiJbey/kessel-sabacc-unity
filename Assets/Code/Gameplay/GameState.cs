using UnityEngine;

namespace KesselSabacc.Gameplay
{
	public abstract class GameState
	{
		/// <summary>
		/// Called when entering this state
		/// </summary>
		public virtual async Awaitable OnEnter(KesselSabaccGameController gameController)
		{
			await Awaitable.NextFrameAsync();
		}

		/// <summary>
		/// Called when exiting this state
		/// </summary>
		public virtual async Awaitable OnExit(KesselSabaccGameController gameController)
		{
			await Awaitable.NextFrameAsync();
		}

		/// <summary>
		/// Handle updates.
		/// </summary>
		public virtual void OnUpdate(KesselSabaccGameController gameController)
		{
			return;
		}

		/// <summary>
		/// Handle input
		/// </summary>
		public virtual void OnInput(KesselSabaccGameController gameController)
		{
			return;
		}

		public virtual void OnRoundAdvanced(KesselSabaccGameController gameController)
		{
			return;
		}

		public virtual void OnTurnAdvanced(KesselSabaccGameController gameController)
		{
			return;
		}

		public virtual void OnTurnTakerAdvanced(KesselSabaccGameController gameController)
		{
			return;
		}
	}
}
