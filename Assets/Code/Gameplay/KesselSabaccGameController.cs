using System.Collections;
using System.Collections.Generic;
using KesselSabacc.Gameplay.AI;
using KesselSabacc.Gameplay.GameStates;
using KesselSabacc.Model;
using KesselSabacc.Views;
using UnityEngine;

namespace KesselSabacc.Gameplay
{
	public class KesselSabaccGameController : MonoBehaviour
	{
		[Header( "References" )]
		public KesselSabaccGameView uiView;

		[Header( "Configuration Settings" )]
		public DeckConfiguration defaultDeckConfig;

		[Header( "Asset References" )]
		public GameObject humanPlayerPrefab;
		public GameObject cpuPlayerPrefab;
		public AIStrategy defaultAIStrategy;

		private GameState _currentGameState = null;
		private bool _isSwitchingState = false;
		private List<PlayerController> _players = new();

		public PresenterCommandSystem CommandSystem { get; } = new();
		public KesselSabaccGameModel Model { get; private set;}
		public IReadOnlyList<PlayerController> Players => _players;

		private IEnumerator Start()
		{
			Model = new KesselSabaccGameModel();
			yield return InitializeGame();
		}

		private void Update()
		{
			CommandSystem.Update();
			if ( _isSwitchingState ) return;
			_currentGameState?.OnInput( this );
			_currentGameState?.OnUpdate( this );
		}

		private async Awaitable InitializeGame()
		{
			var loadingScreen = ApplicationManager.Instance.LoadingScreen;
			loadingScreen.Show();
			await Awaitable.NextFrameAsync();

			if ( NewGameManager.Instance.Data == null )
			{
				NewGameManager.Instance.CreateNewGame();
			}

			NewGameData newGameData = NewGameManager.Instance.Data;

			// Set the deck
			newGameData.deck = (newGameData.deck != null) ? newGameData.deck : defaultDeckConfig;
			Model.SetDeckConfig( newGameData.deck.GetCardCountsByType() );

			// Add human player
			var humanPlayerModel = new Player( "Player 1", newGameData.numChips );
			var humanPlayerController = Instantiate( humanPlayerPrefab ).GetComponent<HumanController>();
			humanPlayerController.Initialize( 0, humanPlayerModel, this );
			Model.AddPlayer( humanPlayerModel );
			_players.Add( humanPlayerController );

			// Add CPU player(s)
			for ( int i = 1; i < newGameData.numPlayers; i++ )
			{
				var cpuPlayerModel = new Player( $"CPU {i}", newGameData.numChips );
				var cpuPlayerController = Instantiate( cpuPlayerPrefab ).GetComponent<AIController>();
				cpuPlayerController.Strategy = defaultAIStrategy;
				cpuPlayerController.Initialize( i, cpuPlayerModel, this );
				Model.AddPlayer( cpuPlayerModel );
				_players.Add( cpuPlayerController );
			}

			uiView.Initialize( this );
			await Awaitable.NextFrameAsync();

			loadingScreen.Hide();
			await Awaitable.NextFrameAsync();

			StartGame();
		}

		public void StartGame()
		{
			GoToDealingState();
		}

		public void GoToDealingState()
		{
			StartCoroutine( SetGameState( new DealingState() ) );
		}

		public void GoToTurnTakingState()
		{
			StartCoroutine( SetGameState( new TurnTakingState() ) );
		}

		public void GoToRoundOverState()
		{
			StartCoroutine( SetGameState( new RoundOverState() ) );
		}

		public void GoToGameOverState()
		{
			StartCoroutine( SetGameState( new GameOverState() ) );
		}

		private async Awaitable SetGameState(GameState newState)
		{
			_isSwitchingState = true;

			if ( _currentGameState != null )
			{
				await _currentGameState.OnExit( this );
			}

			_currentGameState = newState;

			await _currentGameState.OnEnter( this );

			_isSwitchingState = false;
		}

		public void AdvanceTurnTaker()
		{
			Model.AdvanceTurnTaker();
			_currentGameState.OnTurnTakerAdvanced( this );
		}

		public void AdvanceRound()
		{
			Model.AdvanceRound();
			_currentGameState.OnRoundAdvanced( this );
		}

		public void AdvanceTurn()
		{
			Model.AdvanceTurn();
			_currentGameState.OnTurnAdvanced( this );
		}

		public void GoToMainMenu()
		{
			SceneController.Instance
				.NewTransition()
				.Unload( SceneDatabase.Slots.SessionContent )
				.Load( SceneDatabase.Slots.Menu, SceneDatabase.Scenes.MainMenu )
				.WithOverlay()
				.Perform();
		}
	}
}
