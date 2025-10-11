using UnityEngine;

namespace LoveHina
{
	/// <summary>
	/// Configuration settings for the game.
	/// </summary>
	[CreateAssetMenu( fileName = "GameConfig", menuName = "Kessel Sabacc/Game Config" )]
	public class GameConfig : ScriptableObject
	{
		public int startingChips = 4;
		public bool shiftTokensEnabled = false;
		public int numberOfPlayers = 4;
	}
}
