using KesselSabacc.Gameplay;
using KesselSabacc.Model;
using KesselSabacc.UI;
using UnityEngine;

namespace KesselSabacc.Views
{
	public class KesselSabaccGameView : MonoBehaviour
	{
		[Header( "UI References" )]
		public GameHUD hud;
		public DrawCardUI drawCardUI;
		public ShiftTokenTargetSelectionUI shiftTokenTargetSelectionUI;
		public DiscardCardUI discardCardUI;
		public RoundNotificationUI roundNotificationUI;
		public ShiftTokenNotificationUI shiftTokenNotificationUI;
		public DisqualifiedNotificationUI disqualifiedNotificationUI;
		public GameOverNotificationUI gameOverNotificationUI;
		public DiceRollUI diceRollUI;
		public RoundEndUI roundEndUI;

		[Header( "View References" )]
		public TableView tableView;

		[Header( "Asset References" )]
		public GameObject cardViewPrefab;
		public DeckConfiguration deckConfig;

		public static KesselSabaccGameView Instance { get; private set; }

		private void Awake()
		{
			if ( Instance != null )
			{
				Destroy( this );
				return;
			}
			Instance = this;
		}

		private void Start()
		{
			drawCardUI.Hide();
			shiftTokenTargetSelectionUI.Hide();
			discardCardUI.Hide();
			roundNotificationUI.Hide();
			shiftTokenNotificationUI.Hide();
			disqualifiedNotificationUI.Hide();
			gameOverNotificationUI.Hide();
			diceRollUI.Hide();
			roundEndUI.Hide();
		}

		private void OnDestroy()
		{
			if ( Instance == this )
			{
				Instance = null;
			}
		}

		public void Initialize(KesselSabaccGameController gameController)
		{
			hud.Initialize( gameController.Model, this, 0 );
			tableView.Initialize( gameController );
		}

		public CardView SpawnCard(Card card, Vector3 position, Quaternion rotation)
		{
			CardView cardView = Instantiate( cardViewPrefab, position, rotation ).GetComponent<CardView>();
			cardView.Initialize( card, GetCardFront( card.Suit, card.CardType ), GetCardBack( card.Suit ) );
			return cardView;
		}

		public Sprite GetCardBack(CardSuit suit)
		{
			return (suit == CardSuit.BLOOD) ? deckConfig.bloodCardBack : deckConfig.sandCardBack;
		}

		public Sprite GetCardFront(CardSuit suit, CardType cardType)
		{
			switch ( cardType )
			{
				case CardType.SYLOP:
					return (suit == CardSuit.BLOOD) ?
						deckConfig.sylopCards.bloodFront
						: deckConfig.sylopCards.sandFront;
				case CardType.ONE:
					return (suit == CardSuit.BLOOD) ?
						deckConfig.oneCards.bloodFront
						: deckConfig.oneCards.sandFront;
				case CardType.TWO:
					return (suit == CardSuit.BLOOD) ?
						deckConfig.twoCards.bloodFront
						: deckConfig.twoCards.sandFront;
				case CardType.THREE:
					return (suit == CardSuit.BLOOD) ?
						deckConfig.threeCards.bloodFront
						: deckConfig.threeCards.sandFront;
				case CardType.FOUR:
					return (suit == CardSuit.BLOOD) ?
						deckConfig.fourCards.bloodFront
						: deckConfig.fourCards.sandFront;
				case CardType.FIVE:
					return (suit == CardSuit.BLOOD) ?
						deckConfig.fiveCards.bloodFront
						: deckConfig.fiveCards.sandFront;
				case CardType.SIX:
					return (suit == CardSuit.BLOOD) ?
						deckConfig.sixCards.bloodFront
						: deckConfig.sixCards.sandFront;
				case CardType.IMPOSTER:
					return (suit == CardSuit.BLOOD) ?
						deckConfig.imposterCards.bloodFront
						: deckConfig.imposterCards.sandFront;
				default:
					throw new System.ArgumentException( "Unsupported suit or card type" );
			}
		}
	}
}
