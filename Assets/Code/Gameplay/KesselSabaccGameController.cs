using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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


		[Header( "Animation Settings" )]
		public float deckSpawnDuration = 1f;

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
			var loadingScreen = ApplicationManager.Instance.LoadingScreen;
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

			_ = AnimateDeckSpawn( uiView.tableView.SandDeckView, Model.SandDeck );
			_ = AnimateDeckSpawn( uiView.tableView.BloodDeckView, Model.BloodDeck );
			yield return new WaitForSeconds( deckSpawnDuration );
			yield return null;
		}

		public async Task AnimateDeckSpawn(CardStackView stackView, CardStack model)
		{
			int totalCards = model.Cards.Count;
			for ( int i = 0; i < totalCards; i++ )
			{
				Card card = model.Cards[i];
				CardView cardView = uiView.SpawnCard( card, stackView.transform.position, stackView.transform.rotation );
				cardView.ShowBack();
				stackView.AddCard( cardView );

				await Awaitable.WaitForSecondsAsync( deckSpawnDuration / totalCards );
			}
		}

		public IEnumerator PlayDealingSequence()
		{
			TableView tableView = uiView.tableView;

			yield return ResetDecksAndPiles();
			yield return new WaitForSeconds( .500f );

			yield return DiscardTopCardOfDeck(
				tableView.SandDeckView, tableView.SandDiscardPileView );

			yield return DiscardTopCardOfDeck(
				tableView.BloodDeckView, tableView.BloodDiscardPileView );

			yield return new WaitForSeconds( .500f );

			for ( int i = 0; i < Model.Players.Count; i++ )
			{
				var player = Model.Players[i];
				if ( player.IsDisqualified ) continue;
				yield return DealCardToPlayer( tableView.SandDeckView, i );
				yield return DealCardToPlayer( tableView.BloodDeckView, i );
				yield return new WaitForSeconds( .500f );
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

			CardSortingSystem.Instance.AddCardToZone( cardView, CardZone.Hand );

			yield return cardView.MoveCardToPosition(
				playerHand.transform.position,
				playerHand.transform.rotation.eulerAngles
			);

			yield return playerHand.AddCard( cardView );

			if ( playerIndex == 0 )
			{
				yield return cardView.ShowFrontAsync();
			}
			else
			{
				yield return cardView.ShowBackAsync();
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

			CardSortingSystem.Instance.AddCardToZone( cardView, CardZone.Discard );

			yield return cardView.MoveCardToPosition(
				discardPile.transform.position,
				discardPile.transform.rotation.eulerAngles
			);

			yield return cardView.ShowFrontAsync();

			discardPile.Model.Add( card );

			discardPile.AddCard( cardView );

			onEnd?.Invoke();
		}

		public IEnumerator DiscardTopCardOfDeck(CardStackView deck, CardStackView discardPile)
		{
			CardView cardView = deck.Pop();
			Card card = deck.Model.Pop();

			yield return cardView.Flip();

			CardSortingSystem.Instance.AddCardToZone( cardView, CardZone.Discard );

			yield return cardView.MoveCardToPosition(
				discardPile.transform.position,
				discardPile.transform.rotation.eulerAngles
			);

			discardPile.Model.Add( card );

			discardPile.AddCard( cardView );
		}
	}
}
