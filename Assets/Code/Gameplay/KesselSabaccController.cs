using System.Collections.Generic;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Gameplay
{
	public class KesselSabaccController
	{
		private Model.KesselSabacc _model;
		private List<PlayerController> _players;
		private DeckConfiguration _deckConfig;

		public Model.KesselSabacc Model => _model;
		public IReadOnlyList<PlayerController> Players => _players;


		public KesselSabaccController(Model.KesselSabacc model, DeckConfiguration deckConfig)
		{
			_model = model;
			_players = new List<PlayerController>();
			_deckConfig = deckConfig;
		}

		public void StartGame()
		{

		}

		public void IncrementTurnTaker()
		{

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
				cardType,
				GetCardFront( suit, cardType ),
				GetCardBack( suit )
			);
		}

		public Sprite GetCardBack(CardSuit suit)
		{
			return (suit == CardSuit.BLOOD) ? _deckConfig.bloodCardBack : _deckConfig.sandCardBack;
		}

		public Sprite GetCardFront(CardSuit suit, CardType cardType)
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
