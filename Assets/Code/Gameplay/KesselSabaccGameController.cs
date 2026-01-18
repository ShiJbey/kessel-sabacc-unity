using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
			uiView.tableView.BloodDeckView.Clear();
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
			uiView.tableView.SandDeckView.Clear();
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
			uiView.tableView.SandDiscardPileView.Clear();
			_model.SandDiscardPile.Clear();

			uiView.tableView.BloodDiscardPileView.Clear();
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
		public IEnumerator ResetDecksAndPiles()
		{
			Debug.Log( "Resetting Blood and Sand decks." );
			ResetBloodDeck();
			ResetSandDeck();
			ResetDiscardPiles();

			var sandDeckCoroutine = StartCoroutine( uiView.tableView.SandDeckView.AnimateDeckSpawn() );
			var bloodDeckCoroutine = StartCoroutine( uiView.tableView.BloodDeckView.AnimateDeckSpawn() );
			yield return null; // Give the above coroutines a chance to start

			yield return new WaitUntil( () =>
				!uiView.tableView.SandDeckView.IsAnimating
				&& !uiView.tableView.BloodDeckView.IsAnimating
			);
		}

		public void ClearHands()
		{
			foreach ( PlayerController playerController in _players )
			{
				playerController.Model.ClearHand();
				uiView.tableView.playerHands[playerController.PlayerIndex].Clear();
			}
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
			var player = new Model.Player( "Player 1", newGameData.numChips );
			AddPlayer( player );
			AddPlayerController( new HumanController( 0, player ) );

			// Add CPU player(s)
			for ( int i = 1; i < newGameData.numPlayers; i++ )
			{
				var cpu = new Model.Player( $"CPU {i}", newGameData.numChips );
				AddPlayer( cpu );
				AddPlayerController( new SimpleAIController( i, cpu ) );
			}
		}

		public IEnumerator ResetCardStacks()
		{
			yield return null;
		}

		public IEnumerator DealCardToPlayer(CardStackView deck, int playerIndex, Action<CardView> onEnd = null)
		{
			CardView cardView = deck.Pop();
			Card card = deck.Model.Pop();

			Model.Players[playerIndex].AddCardToHand( card );

			HandView playerHand = uiView.tableView.playerHands[playerIndex];

			yield return MoveCardToPosition( cardView, playerHand.transform.position );

			yield return playerHand.AddCard( cardView );

			if ( playerIndex == 0 )
			{
				yield return cardView.ShowFrontAsync();
			}

			onEnd?.Invoke( cardView );
		}

		public IEnumerator DiscardCardFromPlayer(int playerIndex, Card card, Action onEnd = null)
		{

			CardView cardView = uiView.tableView.playerHands[playerIndex].GetCard( card );

			CardStackView discardPile = card.Suit == CardSuit.SAND ?
				uiView.tableView.SandDiscardPileView
				: uiView.tableView.BloodDiscardPileView;

			Model.Players[playerIndex].DiscardCardFromHand( card );

			yield return uiView.tableView.playerHands[playerIndex].RemoveCard( card );

			yield return MoveCardToPosition( cardView, discardPile.transform.position );

			discardPile.Model.Add( card );

			yield return discardPile.AddCard( cardView );

			onEnd?.Invoke();
		}

		public IEnumerator DiscardTopCardOfDeck(CardStackView deck, CardStackView discardPile)
		{
			CardView cardView = deck.Pop();
			Card card = deck.Model.Pop();

			yield return cardView.Flip();

			yield return MoveCardToPosition( cardView, discardPile.transform.position );

			discardPile.Model.Add( card );

			yield return discardPile.AddCard( cardView );
		}

		public IEnumerator MoveCardToPosition(CardView card, Vector3 position)
		{
			var sequence = DOTween.Sequence();

			sequence.Append(
				card.transform.DOMove( position, 0.4f ).SetEase( Ease.OutQuad )
			);

			yield return sequence.WaitForCompletion();
		}
	}
}
