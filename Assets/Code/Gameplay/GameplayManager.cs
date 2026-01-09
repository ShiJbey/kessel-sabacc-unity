using KesselSabacc.Gameplay.GameStates;
using KesselSabacc.Model;
using KesselSabacc.UI;
using UnityEngine;

namespace KesselSabacc.Gameplay
{
	public class GameplayManager : MonoBehaviour
	{
		[SerializeField]
		private CardView _cardViewPrefab;

		private IGameState _currentGameState = null;
		private bool _isSwitchingState = false;

		public KesselSabaccController GameController { get; set; }
		public static GameplayManager Instance { get; private set; }

		private void Awake()
		{
			if ( Instance != null )
			{
				Destroy( gameObject );
				return;
			}
			Instance = this;
		}

		private void Update()
		{
			if ( _isSwitchingState ) return;
			_currentGameState?.OnInput();
			_currentGameState?.OnUpdate();
		}

		public void StartGame()
		{
			GoToDealingState();
		}

		public void GoToDealingState()
		{
			SetGameState( new DealingState() );
		}

		public void GoToTurnTakingState()
		{
			SetGameState( new TurnTakingState() );
		}

		public void GoToRoundOverState()
		{
			SetGameState( new RoundOverState() );
		}

		public void GoToGameOverState()
		{
			SetGameState( new GameOverState() );
		}

		private async void SetGameState(IGameState newState)
		{
			_isSwitchingState = true;

			if ( _currentGameState != null )
			{
				await _currentGameState.OnExit();
			}

			_currentGameState = newState;

			await _currentGameState.OnEnter();

			_isSwitchingState = false;
		}

		public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation)
		{
			CardView cardView = Instantiate( _cardViewPrefab, position, rotation );
			cardView.Initialize( card );
			return cardView;
		}
	}
}
