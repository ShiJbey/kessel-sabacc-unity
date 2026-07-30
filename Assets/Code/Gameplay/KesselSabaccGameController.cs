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

		[Header( "Configuration Settings" )]
		public DeckConfiguration defaultDeckConfig;

		[Header( "Asset References" )]
		public GameObject cardViewPrefab;
		public GameObject humanPlayerPrefab;
		public GameObject cpuPlayerPrefab;

		private GameState _currentGameState = null;
		private bool _isSwitchingState = false;
		private List<PlayerController> _players = new();
		private KesselSabaccGameModel _model;
		private DeckConfiguration _deckConfig;

		public KesselSabaccGameModel Model => _model;
		public IReadOnlyList<PlayerController> Players => _players;

		private IEnumerator Start()
		{
			_model = new KesselSabaccGameModel();
			yield return InitializeGame();
		}

		private void Update()
		{
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
			_deckConfig = newGameData.deck;

			// Add human player
			var humanPlayerModel = new Player( "Player 1", newGameData.numChips );
			var humanPlayerController = Instantiate( humanPlayerPrefab ).GetComponent<HumanController>();
			humanPlayerController.Initialize( 0, humanPlayerModel, this );
			_model.AddPlayer( humanPlayerModel );
			_players.Add( humanPlayerController );

			// Add CPU player(s)
			for ( int i = 1; i < newGameData.numPlayers; i++ )
			{
				var cpuPlayerModel = new Player( $"CPU {i}", newGameData.numChips );
				var cpuPlayerController = Instantiate( cpuPlayerPrefab ).GetComponent<SimpleAIController>();
				cpuPlayerController.Initialize( i, cpuPlayerModel, this );
				_model.AddPlayer( cpuPlayerModel );
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

		public void ResetDrawPiles()
		{
			uiView.tableView.SandDeckView.Clear();
			_model.SandDeck.Clear();

			uiView.tableView.BloodDeckView.Clear();
			_model.BloodDeck.Clear();

			foreach ( var entry in _deckConfig.cards )
			{
				for ( int i = 0; i < entry.count; i++ )
				{
					_model.SandDeck.Add(
						new Card( CardSuit.SAND, entry.cardType, entry.sandFront, _deckConfig.sandCardBack ) );

					_model.BloodDeck.Add(
						new Card( CardSuit.BLOOD, entry.cardType, entry.bloodFront, _deckConfig.bloodCardBack ) );
				}
			}

			_model.SandDeck.Shuffle();
			_model.BloodDeck.Shuffle();
		}

		public void ResetDiscardPiles()
		{
			uiView.tableView.SandDiscardPileView.Clear();
			_model.SandDiscardPile.Clear();

			uiView.tableView.BloodDiscardPileView.Clear();
			_model.BloodDiscardPile.Clear();
		}

		public CardView SpawnCard(Card card, Vector3 position, Quaternion rotation)
		{
			CardView cardView = Instantiate( cardViewPrefab, position, rotation ).GetComponent<CardView>();
			cardView.Initialize( card );
			return cardView;
		}

		/// <summary>
		/// Reset the cards within the blood and sand decks, clear swap stacks.
		/// </summary>
		public async Awaitable ResetDecksAndPiles()
		{
			ResetDrawPiles();
			ResetDiscardPiles();

			await Task.WhenAll(
				AnimateDeckSpawn( uiView.tableView.SandDeckView, Model.SandDeck ).AsTask(),
				AnimateDeckSpawn( uiView.tableView.BloodDeckView, Model.BloodDeck ).AsTask()
			);
		}

		public async Awaitable AnimateDeckSpawn(CardStackView stackView, CardStack model)
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

		public async Awaitable PlayDealingSequence()
		{
			TableView tableView = uiView.tableView;

			await ResetDecksAndPiles();

			await DiscardTopCardOfDeck(
				tableView.SandDeckView, tableView.SandDiscardPileView );

			await DiscardTopCardOfDeck(
				tableView.BloodDeckView, tableView.BloodDiscardPileView );

			for ( int i = 0; i < Model.Players.Count; i++ )
			{
				var player = Model.Players[i];
				if ( player.IsDisqualified ) continue;
				await DealCardToPlayer( tableView.SandDeckView.Model, i );
				await DealCardToPlayer( tableView.BloodDeckView.Model, i );
			}
		}

		public void ClearHands()
		{
			foreach ( PlayerController playerController in _players )
			{
				playerController.Model.ClearHand();
				uiView.tableView.playerHands[playerController.PlayerIndex].Clear();
			}
		}

		public async Awaitable DealCardToPlayer(CardStack deck, int playerIndex)
		{
			CardView cardView = GetCardStackView( deck ).Pop();
			Card card = deck.Pop();

			Model.Players[playerIndex].AddCardToHand( card );

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

		private CardStackView GetCardStackView(CardStack cardStack)
		{
			if ( cardStack == uiView.tableView.SandDiscardPileView.Model )
			{
				return uiView.tableView.SandDiscardPileView;
			}
			else if ( cardStack == uiView.tableView.SandDeckView.Model )
			{
				return uiView.tableView.SandDeckView;
			}
			else if ( cardStack == uiView.tableView.BloodDeckView.Model )
			{
				return uiView.tableView.BloodDeckView;
			}
			else if ( cardStack == uiView.tableView.BloodDiscardPileView.Model )
			{
				return uiView.tableView.BloodDiscardPileView;
			}

			return null;
		}

		public async Awaitable DiscardCardFromPlayer(int playerIndex, Card card)
		{

			CardView cardView = uiView.tableView.playerHands[playerIndex].GetCard( card );

			CardStackView discardPile = card.Suit == CardSuit.SAND ?
				uiView.tableView.SandDiscardPileView
				: uiView.tableView.BloodDiscardPileView;

			Model.Players[playerIndex].DiscardCardFromHand( card );

			await uiView.tableView.playerHands[playerIndex].RemoveCard( card );

			await cardView.MoveCardToPosition(
				discardPile.transform.position,
				discardPile.transform.rotation.eulerAngles
			);

			discardPile.AddCard( cardView );

			await cardView.ShowFrontAsync();
		}

		public async Awaitable DiscardTopCardOfDeck(CardStackView deck, CardStackView discardPile)
		{
			CardView cardView = deck.Pop();
			deck.Model.Pop();

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
	}
}
