// Implement custom AsTask extension methods to wrap Awaitable in Task
using System.Threading.Tasks;
using UnityEngine;

namespace KesselSabacc.Utils
{
	public static class AwaitableExtensions
	{
		public static async Task AsTask(this Awaitable a)
		{
			await a;
		}

		public static async Task<T> AsTask<T>(this Awaitable<T> a)
		{
			return await a;
		}
	}
}
