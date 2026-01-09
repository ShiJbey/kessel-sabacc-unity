using System.Threading.Tasks;
using UnityEngine;

namespace KesselSabacc.Gameplay.GameStates
{
	public class DealingState : IGameState
	{
		private KesselSabaccController _gameController;

		public Task OnEnter()
		{
			_gameController = GameplayManager.Instance.GameController;
			_gameController.ResetDecksAndPiles();
			Debug.Log( "Dealing Cards" );


			var sandCard = _gameController.Model.SandDeck.Pop();
			sandCard.IsFaceUp = true;
			_gameController.Model.SandDiscardPile.Add( sandCard );
			var bloodCard = _gameController.Model.BloodDeck.Pop();
			bloodCard.IsFaceUp = true;
			_gameController.Model.BloodDiscardPile.Add( bloodCard );

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


			// foreach ( var player in _gameController.Players )
			// {
			// 	if ( player.Model.IsDisqualified ) continue;

			// 	player.Model.AddCardToHand( _gameController.Model.BloodDeck.Pop() );
			// 	player.Model.AddCardToHand( _gameController.Model.SandDeck.Pop() );
			// }

			// _gameController.Model.BloodDiscardPile.Add(
			// 	_gameController.Model.BloodDeck.Pop()
			// );

			// _gameController.Model.SandDiscardPile.Add(
			// 	_gameController.Model.SandDeck.Pop()
			// );

			// GameplayManager.Instance.GoToTurnTakingState();
		}
	}
}
