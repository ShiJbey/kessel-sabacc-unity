using System;
using System.Collections.Generic;

namespace KesselSabacc.Model
{
	public class RoundResultList
	{
		private List<PlayerRoundResult> _results = new();

		public IReadOnlyList<PlayerRoundResult> Results => _results;

		public event Action<PlayerRoundResult> OnResultAdded;
		public event Action OnResultsCleared;
		public event Action OnSort;

		public void Add(PlayerRoundResult result)
		{
			_results.Add( result );
			OnResultAdded?.Invoke( result );
		}

		public void Sort()
		{
			_results.Sort( (a, b) => b.CompareTo( a ) );
			OnSort?.Invoke();
		}

		public void Clear()
		{
			_results.Clear();
			OnResultsCleared?.Invoke();
		}
	}
}
