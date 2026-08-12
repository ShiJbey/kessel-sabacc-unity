using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using KesselSabacc.Gameplay.AI;
using KesselSabacc.Gameplay.GameStates;
using KesselSabacc.Model;
using KesselSabacc.Views;
using KesselSabacc.Utils;
using UnityEngine;

namespace KesselSabacc.Gameplay
{
	public class KesselSabaccGameController : MonoBehaviour
	{
		[Header( "References" )]
		public KesselSabaccGameView uiView;

		[Header( "Animation Settings" )]
		public float deckSpawnDuration = 1f;
		public float handRevealDelay = 2.5f;

		[Header( "Configuration Settings" )]
		public DeckConfiguration defaultDeckConfig;
		public Color[] playerColors;

		[Header( "Asset References" )]
		public GameObject cardViewPrefab;
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

		private void OnDestroy()
		{
			Model.OnCardDrawn -= OnCardDrawn;
			Model.OnCardDiscarded -= OnCardDiscarded;
			Model.OnDrawPilesCleared -= OnDrawPilesCleared;
			Model.OnDiscardPilesCleared -= OnDiscardPilesCleared;
			Model.OnDrawPilesReset -= OnDrawPilesReset;
			Model.OnHandDealt -= OnHandDealt;
			Model.OnHandsCleared -= OnHandsCleared;
		}

		private async Awaitable InitializeGame()
		{
			Model.OnCardDrawn += OnCardDrawn;
			Model.OnCardDiscarded += OnCardDiscarded;
			Model.OnDrawPilesCleared += OnDrawPilesCleared;
			Model.OnDiscardPilesCleared += OnDiscardPilesCleared;
			Model.OnDrawPilesReset += OnDrawPilesReset;
			Model.OnHandDealt += OnHandDealt;
			Model.OnHandsCleared += OnHandsCleared;

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

		private void OnDrawPilesCleared()
		{
			uiView.tableView.SandDeckView.Clear();
			uiView.tableView.BloodDeckView.Clear();
		}

		private void OnDiscardPilesCleared()
		{
			uiView.tableView.SandDiscardPileView.Clear();
			uiView.tableView.BloodDiscardPileView.Clear();
		}

		private CardView SpawnCard(Card card, Vector3 position, Quaternion rotation)
		{
			CardView cardView = Instantiate( cardViewPrefab, position, rotation ).GetComponent<CardView>();
			cardView.Initialize( card );
			return cardView;
		}

		private void OnDrawPilesReset()
		{
			CommandSystem.QueueCommand(
				PresenterCommandSystem.Func(
					async () =>
					{
						await Task.WhenAll(
							AnimateDeckSpawn( uiView.tableView.SandDeckView, Model.SandDeck ).AsTask(),
							AnimateDeckSpawn( uiView.tableView.BloodDeckView, Model.BloodDeck ).AsTask()
						);

						// Temporarily add a sand card to the draw deck to animate it moving
						// to the discard pile
						Card sandCard = Model.SandDiscardPile.Cards[0];
						CardView sandCardView = SpawnCard(
							sandCard,
							uiView.tableView.SandDeckView.transform.position,
							uiView.tableView.SandDeckView.transform.rotation
						);
						sandCardView.ShowBack();
						uiView.tableView.SandDeckView.AddCard( sandCardView );

						// Temporarily add a blood card to the draw deck to animate it moving
						// to the discard pile
						Card bloodCard = Model.BloodDiscardPile.Cards[0];
						CardView bloodCardView = SpawnCard(
							bloodCard,
							uiView.tableView.BloodDeckView.transform.position,
							uiView.tableView.BloodDeckView.transform.rotation
						);
						bloodCardView.ShowBack();
						uiView.tableView.BloodDeckView.AddCard( bloodCardView );

						await Task.WhenAll(
							DiscardTopCardOfDeck(
								uiView.tableView.SandDeckView, uiView.tableView.SandDiscardPileView ).AsTask(),
							DiscardTopCardOfDeck(
								uiView.tableView.BloodDeckView, uiView.tableView.BloodDiscardPileView ).AsTask()
						);
					}
				)
			);
		}

		private async Awaitable AnimateDeckSpawn(CardStackView stackView, CardStack model)
		{
			int totalCards = model.Cards.Count;
			for ( int i = 0; i < totalCards; i++ )
			{
				Card card = model.Cards[i];
				CardView cardView = SpawnCard( card, stackView.transform.position, stackView.transform.rotation );
				cardView.ShowBack();
				stackView.AddCard( cardView );

				await Awaitable.WaitForSecondsAsync( deckSpawnDuration / totalCards );
			}
		}

		public void RevealHands(KesselSabaccGameController gameController)
		{
			CommandSystem.QueueCommand(
				PresenterCommandSystem.Func(
					async () =>
					{
						foreach ( HandView handView in gameController.uiView.tableView.playerHands )
						{
							handView.RevealHand();
							await Awaitable.WaitForSecondsAsync( handRevealDelay );
						}
					}
				)
			);
		}

		private void OnHandDealt(HandDealtEventData eventData)
		{
			CommandSystem.QueueCommand(
				PresenterCommandSystem.Func(
					async () =>
					{
						await Task.WhenAll(
							DealCardToPlayer( CardStack.DeckKind.SAND_DRAW, eventData.playerIndex ).AsTask(),
							DealCardToPlayer( CardStack.DeckKind.BLOOD_DRAW, eventData.playerIndex ).AsTask()
						);
						await Awaitable.WaitForSecondsAsync( 0.5f );
					}
				)
			);
		}

		public void PlayDealingSequence()
		{
			Model.ClearHands();
			Model.ClearDiscardPiles();
			Model.ClearDrawPiles();
			Model.ResetDrawPiles();
			Model.DealHands();
		}

		private void OnHandsCleared()
		{
			foreach ( PlayerController playerController in _players )
			{
				uiView.tableView.playerHands[playerController.PlayerIndex].Clear();
			}
		}

		private async Awaitable DealCardToPlayer(CardStack.DeckKind deckKind, int playerIndex)
		{
			CardStackView cardStackView = GetCardStackViewOfKind( deckKind );
			CardView cardView = cardStackView.Pop();

			HandView playerHand = uiView.tableView.playerHands[playerIndex];

			CardSortingSystem.Instance.AddCardToZone( cardView, CardZone.Hand );

			await cardView.MoveCardToPosition(
				playerHand.transform.position,
				playerHand.transform.rotation.eulerAngles
			);

			await playerHand.AddCard( cardView );

			if ( playerIndex == 0 )
			{
				await cardView.ShowFrontAsync();
			}
			else
			{
				await cardView.ShowBackAsync();
			}
		}

		private async Awaitable DiscardCardFromPlayer(int playerIndex, int cardIndex, CardSuit suit)
		{
			CardView cardView = uiView.tableView.playerHands[playerIndex].Cards[cardIndex];

			CardStackView discardPileView = suit == CardSuit.SAND ?
				uiView.tableView.SandDiscardPileView
				: uiView.tableView.BloodDiscardPileView;

			await uiView.tableView.playerHands[playerIndex].RemoveCard( cardIndex );

			await cardView.MoveCardToPosition(
				discardPileView.transform.position,
				discardPileView.transform.rotation.eulerAngles
			);

			discardPileView.AddCard( cardView );

			await cardView.ShowFrontAsync();
		}

		private async Awaitable DiscardTopCardOfDeck(CardStackView deck, CardStackView discardPile)
		{
			CardView cardView = deck.Pop();

			await cardView.Flip();

			await cardView.MoveCardToPosition(
				discardPile.transform.position,
				discardPile.transform.rotation.eulerAngles
			);

			discardPile.AddCard( cardView );
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

		private void OnCardDrawn(CardDrawnEventData eventData)
		{
			CommandSystem.QueueCommand(
				PresenterCommandSystem.Func(
					async () =>
					{
						await DealCardToPlayer( eventData.deck, eventData.playerIndex );
					}
				)
			);
		}

		private void OnCardDiscarded(CardDiscardedEventData eventData)
		{
			CommandSystem.QueueCommand(
				PresenterCommandSystem.Func(
					async () =>
					{
						await DiscardCardFromPlayer( eventData.playerIndex, eventData.cardIndex, eventData.cardSuit );
					}
				)
			);
		}

		private CardStackView GetCardStackViewOfKind(CardStack.DeckKind kind)
		{
			switch ( kind )
			{
				case CardStack.DeckKind.SAND_DISCARD:
					return uiView.tableView.SandDiscardPileView;
				case CardStack.DeckKind.SAND_DRAW:
					return uiView.tableView.SandDeckView;
				case CardStack.DeckKind.BLOOD_DISCARD:
					return uiView.tableView.BloodDiscardPileView;
				case CardStack.DeckKind.BLOOD_DRAW:
					return uiView.tableView.BloodDeckView;
				default:
					return null;
			}
		}
	}
}
