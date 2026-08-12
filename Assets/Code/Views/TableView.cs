using System.Threading.Tasks;
using KesselSabacc.Gameplay;
using KesselSabacc.Model;
using KesselSabacc.Utils;
using UnityEngine;

namespace KesselSabacc.Views
{
	public class TableView : MonoBehaviour
	{
		[SerializeField]
		private CardStackView _sandDiscardPileView;
		[SerializeField]
		private CardStackView _sandDeckView;
		[SerializeField]
		private CardStackView _bloodDeckView;
		[SerializeField]
		private CardStackView _bloodDiscardPileView;
		[SerializeField]
		private Transform _cardsContainer;

		[Header( "Animation Settings" )]
		public float deckSpawnDuration = 1f;
		public float handRevealDelay = 1f;

		[Header( "Asset References" )]
		public GameObject cardViewPrefab;

		private DeckConfiguration _deckConfig;
		private Color[] _playerColors;
		private KesselSabaccGameController _gameController;

		public HandView[] playerHands;

		public CardStackView SandDiscardPileView => _sandDiscardPileView;
		public CardStackView SandDeckView => _sandDeckView;
		public CardStackView BloodDeckView => _bloodDeckView;
		public CardStackView BloodDiscardPileView => _bloodDiscardPileView;

		private void OnDestroy()
		{
			_gameController.Model.OnCardDrawn -= OnCardDrawn;
			_gameController.Model.OnCardDiscarded -= OnCardDiscarded;
			_gameController.Model.OnDrawPilesCleared -= OnDrawPilesCleared;
			_gameController.Model.OnDiscardPilesCleared -= OnDiscardPilesCleared;
			_gameController.Model.OnDrawPilesReset -= OnDrawPilesReset;
			_gameController.Model.OnHandDealt -= OnHandDealt;
			_gameController.Model.OnHandsCleared -= OnHandsCleared;
		}

		public void Initialize(KesselSabaccGameController gameController, int playerIndex, Color[] playerColors)
		{
			_gameController = gameController;

			_gameController.Model.OnCardDrawn += OnCardDrawn;
			_gameController.Model.OnCardDiscarded += OnCardDiscarded;
			_gameController.Model.OnDrawPilesCleared += OnDrawPilesCleared;
			_gameController.Model.OnDiscardPilesCleared += OnDiscardPilesCleared;
			_gameController.Model.OnDrawPilesReset += OnDrawPilesReset;
			_gameController.Model.OnHandDealt += OnHandDealt;
			_gameController.Model.OnHandsCleared += OnHandsCleared;

			_sandDiscardPileView.Initialize( CardZone.Discard );
			_sandDeckView.Initialize( CardZone.Deck );
			_bloodDeckView.Initialize( CardZone.Deck );
			_bloodDiscardPileView.Initialize( CardZone.Discard );

			for ( int i = 0; i < gameController.Players.Count; i++ )
			{
				if ( i == playerIndex ) continue;

				playerHands[i].Initialize( gameController.Players[i], playerColors[i] );
			}
		}

		private CardView SpawnCard(Card card, Vector3 position, Quaternion rotation)
		{
			CardView cardView = Instantiate( cardViewPrefab, _cardsContainer, true ).GetComponent<CardView>();
			cardView.transform.position = position;
			cardView.transform.rotation = rotation;
			cardView.Initialize( card );
			return cardView;
		}

		private async Awaitable DealCardToPlayer(CardStack.DeckKind deckKind, int playerIndex)
		{
			CardStackView cardStackView = GetCardStackViewOfKind( deckKind );
			CardView cardView = cardStackView.Pop();

			HandView playerHand = playerHands[playerIndex];

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
			CardView cardView = playerHands[playerIndex].Cards[cardIndex];

			CardStackView discardPileView = suit == CardSuit.SAND ?
				SandDiscardPileView
				: BloodDiscardPileView;

			await playerHands[playerIndex].RemoveCard( cardIndex );

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

		public void RevealHands()
		{
			_gameController.CommandSystem.QueueCommand(
				PresenterCommandSystem.Func(
					async () =>
					{
						for ( int i = 0; i < _gameController.Players.Count; i++ )
						{
							if (_gameController.Players[i].Model.IsDisqualified) continue;
							playerHands[i].RevealHand();
							await Awaitable.WaitForSecondsAsync( handRevealDelay );
						}
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

		private CardStackView GetCardStackViewOfKind(CardStack.DeckKind kind)
		{
			switch ( kind )
			{
				case CardStack.DeckKind.SAND_DISCARD:
					return SandDiscardPileView;
				case CardStack.DeckKind.SAND_DRAW:
					return SandDeckView;
				case CardStack.DeckKind.BLOOD_DISCARD:
					return BloodDiscardPileView;
				case CardStack.DeckKind.BLOOD_DRAW:
					return BloodDeckView;
				default:
					return null;
			}
		}

		#region Event Listeners

		private void OnHandsCleared()
		{
			foreach ( HandView hand in playerHands )
			{
				hand.Clear();
			}
		}

		private void OnDrawPilesCleared()
		{
			SandDeckView.Clear();
			BloodDeckView.Clear();
		}

		private void OnDiscardPilesCleared()
		{
			SandDiscardPileView.Clear();
			BloodDiscardPileView.Clear();
		}

		private void OnHandDealt(HandDealtEventData eventData)
		{
			_gameController.CommandSystem.QueueCommand(
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

		private void OnCardDrawn(CardDrawnEventData eventData)
		{
			_gameController.CommandSystem.QueueCommand(
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
			_gameController.CommandSystem.QueueCommand(
				PresenterCommandSystem.Func(
					async () =>
					{
						await DiscardCardFromPlayer( eventData.playerIndex, eventData.cardIndex, eventData.cardSuit );
					}
				)
			);
		}

		private void OnDrawPilesReset()
		{
			_gameController.CommandSystem.QueueCommand(
				PresenterCommandSystem.Func(
					async () =>
					{
						await Task.WhenAll(
							AnimateDeckSpawn( SandDeckView, _gameController.Model.SandDeck ).AsTask(),
							AnimateDeckSpawn( BloodDeckView, _gameController.Model.BloodDeck ).AsTask()
						);

						// Temporarily add a sand card to the draw deck to animate it moving
						// to the discard pile
						Card sandCard = _gameController.Model.SandDiscardPile.Cards[0];
						CardView sandCardView = SpawnCard(
							sandCard,
							SandDeckView.transform.position,
							SandDeckView.transform.rotation
						);
						sandCardView.ShowBack();
						SandDeckView.AddCard( sandCardView );

						// Temporarily add a blood card to the draw deck to animate it moving
						// to the discard pile
						Card bloodCard = _gameController.Model.BloodDiscardPile.Cards[0];
						CardView bloodCardView = SpawnCard(
							bloodCard,
							BloodDeckView.transform.position,
							BloodDeckView.transform.rotation
						);
						bloodCardView.ShowBack();
						BloodDeckView.AddCard( bloodCardView );

						await Task.WhenAll(
							DiscardTopCardOfDeck(
								SandDeckView, SandDiscardPileView ).AsTask(),
							DiscardTopCardOfDeck(
								BloodDeckView, BloodDiscardPileView ).AsTask()
						);
					}
				)
			);
		}

		#endregion
	}
}
