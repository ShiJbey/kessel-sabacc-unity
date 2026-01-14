using System.Collections;
using KesselSabacc.Views;
using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class RoundOverState : IGameState
	{
		private KesselSabaccGameController _gameController;

		public RoundOverState(KesselSabaccGameController gameController)
		{
			_gameController = gameController;
		}

		public IEnumerator OnEnter()
		{
			yield return RevealHandsAnimation();
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

		private IEnumerator RevealHandsAnimation()
		{
			foreach ( HandView handView in _gameController.uiView.tableView.playerHands )
			{
				foreach ( CardView cardView in handView.Cards )
				{
					yield return cardView.ShowFrontAsync();
				}
				yield return new WaitForSeconds( 1f );
			}

		}
	}
}
