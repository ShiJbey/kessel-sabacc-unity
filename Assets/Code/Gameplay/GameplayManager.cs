using DG.Tweening;
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

		[SerializeField]
		private DeckConfiguration _deckConfig;
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

		public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation, bool showCard = false)
		{
			CardView cardView = Instantiate( _cardViewPrefab, position, rotation );
			cardView.Initialize(
				card, GetCardFront( card.Suit, card.CardType ), GetCardBack( card.Suit )
			);

			if ( showCard == false )
			{
				cardView.ShowBackImmediate();
			}

			// The scaling below is just for fun
			cardView.transform.localScale = Vector3.zero;
			cardView.transform.DOScale( Vector3.one, 0.15f );

			return cardView;
		}

		private Sprite GetCardBack(CardSuit suit)
		{
			return (suit == CardSuit.BLOOD) ? _deckConfig.bloodCardBack : _deckConfig.sandCardBack;
		}

		private Sprite GetCardFront(CardSuit suit, CardType cardType)
		{
			switch ( cardType )
			{
				case CardType.SYLOP:
					return (suit == CardSuit.BLOOD) ?
						_deckConfig.sylopCards.bloodFront
						: _deckConfig.sylopCards.sandFront;
				case CardType.ONE:
					return (suit == CardSuit.BLOOD) ?
						_deckConfig.oneCards.bloodFront
						: _deckConfig.oneCards.sandFront;
				case CardType.TWO:
					return (suit == CardSuit.BLOOD) ?
						_deckConfig.twoCards.bloodFront
						: _deckConfig.twoCards.sandFront;
				case CardType.THREE:
					return (suit == CardSuit.BLOOD) ?
						_deckConfig.threeCards.bloodFront
						: _deckConfig.threeCards.sandFront;
				case CardType.FOUR:
					return (suit == CardSuit.BLOOD) ?
						_deckConfig.fourCards.bloodFront
						: _deckConfig.fourCards.sandFront;
				case CardType.FIVE:
					return (suit == CardSuit.BLOOD) ?
						_deckConfig.fiveCards.bloodFront
						: _deckConfig.fiveCards.sandFront;
				case CardType.SIX:
					return (suit == CardSuit.BLOOD) ?
						_deckConfig.sixCards.bloodFront
						: _deckConfig.sixCards.sandFront;
				case CardType.IMPOSTER:
					return (suit == CardSuit.BLOOD) ?
						_deckConfig.imposterCards.bloodFront
						: _deckConfig.imposterCards.sandFront;
				default:
					throw new System.ArgumentException( "Unsupported suit or card type" );
			}
		}
	}
}
