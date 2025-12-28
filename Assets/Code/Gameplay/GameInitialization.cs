using System.Threading.Tasks;
using UnityEngine;

namespace KesselSabacc.Gameplay
{
	public class GameInitialization : MonoBehaviour
	{
		private void Start()
		{
			InitializeGame();
		}

		private async void InitializeGame()
		{
			await LoadGameConfiguration();
			CreateTestGame();
		}

		private async Task LoadGameConfiguration()
		{

		}

		private void CreateTestGame()
		{

		}
	}
}
