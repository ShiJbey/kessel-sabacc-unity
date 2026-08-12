using UnityEngine;

namespace KesselSabacc.Gameplay
{
	public abstract class PresenterCommand
	{
		public abstract Awaitable Execute();
	}
}
