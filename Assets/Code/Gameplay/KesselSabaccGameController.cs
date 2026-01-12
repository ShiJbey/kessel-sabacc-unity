using System.Collections;
using System.Collections.Generic;
using KesselSabacc.Gameplay.AI;
using KesselSabacc.Gameplay.GameStates;
using KesselSabacc.Model;
using KesselSabacc.UI.Screens;
using KesselSabacc.Views;
using UnityEngine;

namespace KesselSabacc.Gameplay
{
	public class KesselSabaccGameController : MonoBehaviour
	{
		[Header( "References" )]
		public KesselSabaccGameView uiView;


		[Header( "Animation Settings" )]
		public float cardDealSpeed = 0.5f;
		public float delayBetweenCards = 0.3f;
		public float deckSpawnDuration = 1f;
		public float roundPanelDisplayTime = 2f;

		private IGameState _currentGameState = null;
		private bool _isSwitchingState = false;
		private List<PlayerController> _players = new();
		private KesselSabaccGameModel _model;

		public KesselSabaccGameModel Model => _model;
		public IReadOnlyList<PlayerController> Players => _players;

		private void Start()
		{
			_model = new KesselSabaccGameModel();
			StartCoroutine( InitializeGame() );
		}

		private void Update()
		{
			if ( _isSwitchingState ) return;
			_currentGameState?.OnInput();
			_currentGameState?.OnUpdate();
		}

		private IEnumerator InitializeGame()
		{
			yield return new WaitUntil( () => AutoLoadManager.Instance.isReady );

			var loadingScreen = FindFirstObjectByType<LoadingScreen>( FindObjectsInactive.Include );
			loadingScreen.Show();
			yield return null;

			CreateTestGame();
			yield return null;

			uiView.Initialize( this );
			yield return null;

			loadingScreen.Hide();
			yield return null;

			StartGame();
		}

		public void StartGame()
		{
			GoToDealingState();
		}

		public void GoToDealingState()
		{
			StartCoroutine( SetGameState( new DealingState( this ) ) );
		}

		public void GoToTurnTakingState()
		{
			StartCoroutine( SetGameState( new TurnTakingState( this ) ) );
		}

		public void GoToRoundOverState()
		{
			StartCoroutine( SetGameState( new RoundOverState( this ) ) );
		}

		public void GoToGameOverState()
		{
			StartCoroutine( SetGameState( new GameOverState( this ) ) );
		}

		private IEnumerator SetGameState(IGameState newState)
		{
			_isSwitchingState = true;

			if ( _currentGameState != null )
			{
				yield return _currentGameState.OnExit();
			}

			_currentGameState = newState;

			yield return _currentGameState.OnEnter();

			_isSwitchingState = false;
		}

		public void AdvanceTurnTaker()
		{
			Model.AdvanceTurnTaker();
		}

		public void AdvanceRound()
		{
			Model.AdvanceRound();
		}

		public void AdvanceTurn()
		{
			Model.AdvanceTurn();
		}

		public void ResetBloodDeck()
		{
			_model.BloodDeck.Clear();

			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.SYLOP ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.ONE ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.ONE ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.ONE ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.TWO ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.TWO ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.TWO ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.THREE ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.THREE ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.THREE ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.FOUR ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.FOUR ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.FOUR ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.FIVE ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.FIVE ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.FIVE ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.SIX ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.SIX ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.SIX ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.IMPOSTER ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.IMPOSTER ) );
			_model.BloodDeck.Add( CreateCard( CardSuit.BLOOD, CardType.IMPOSTER ) );

			_model.BloodDeck.Shuffle();
		}

		public void ResetSandDeck()
		{
			_model.SandDeck.Clear();

			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.SYLOP ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.ONE ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.ONE ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.ONE ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.TWO ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.TWO ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.TWO ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.THREE ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.THREE ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.THREE ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.FOUR ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.FOUR ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.FOUR ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.FIVE ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.FIVE ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.FIVE ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.SIX ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.SIX ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.SIX ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.IMPOSTER ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.IMPOSTER ) );
			_model.SandDeck.Add( CreateCard( CardSuit.SAND, CardType.IMPOSTER ) );

			_model.SandDeck.Shuffle();
		}

		public void ResetDiscardPiles()
		{
			_model.SandDiscardPile.Clear();
			_model.BloodDiscardPile.Clear();
		}

		public void AddPlayer(Player player)
		{
			_model.AddPlayer( player );
		}

		public void AddPlayerController(PlayerController playerController)
		{
			_players.Add( playerController );
			playerController.Initialize( this );
		}

		/// <summary>
		/// Reset the cards within the blood and sand decks, clear swap stacks.
		/// </summary>
		public void ResetDecksAndPiles()
		{
			Debug.Log( "Resetting Blood and Sand decks." );
			ResetBloodDeck();
			ResetSandDeck();
			ResetDiscardPiles();
		}

		/// <summary>
		/// Deal hands to the players.
		/// </summary>
		public void DealHands()
		{
			Debug.Log( "Dealing hands to the players." );
		}

		public Card CreateCard(CardSuit suit, CardType cardType)
		{
			return new Card(
				suit,
				cardType
			);
		}
		private void CreateTestGame()
		{
			if ( NewGameManager.Instance.Data == null )
			{
				NewGameManager.Instance.CreateNewGame();
			}

			NewGameData newGameData = NewGameManager.Instance.Data;

			Debug.Log(
				$"Creating a new game with {newGameData.numPlayers} players and {newGameData.numChips} chips"
			);

			// Add human player
			var player = new Model.Player( "Player 1" );
			player.Chips = newGameData.numChips;
			AddPlayer( player );
			AddPlayerController( new HumanController( player ) );

			// Add CPU player(s)
			for ( int i = 1; i < newGameData.numPlayers; i++ )
			{
				var cpu = new Model.Player( $"CPU {i}" );
				cpu.Chips = newGameData.numChips;
				AddPlayer( cpu );
				AddPlayerController( new SimpleAIController( cpu ) );
			}
		}
	}
}
