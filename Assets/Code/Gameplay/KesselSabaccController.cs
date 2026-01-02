using System.Collections.Generic;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Gameplay
{
	public class KesselSabaccController
	{
		private Model.KesselSabacc _model;
		private List<PlayerController> _players;

		public Model.KesselSabacc Model => _model;
		public IReadOnlyList<PlayerController> Players => _players;
		public DeckConfiguration DeckConfig { get; set; }

		public KesselSabaccController(Model.KesselSabacc model)
		{
			_model = model;
			_players = new List<PlayerController>();
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

			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.SYLOP ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.ONE ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.ONE ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.ONE ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.TWO ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.TWO ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.TWO ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.THREE ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.THREE ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.THREE ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.FOUR ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.FOUR ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.FOUR ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.FIVE ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.FIVE ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.FIVE ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.SIX ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.SIX ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.SIX ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.IMPOSTER ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.IMPOSTER ) );
			_model.BloodDeck.Add( new Card( CardSuit.BLOOD, CardType.IMPOSTER ) );

			_model.BloodDeck.Shuffle();
		}

		public void ResetSandDeck()
		{
			_model.SandDeck.Clear();

			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.SYLOP ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.ONE ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.ONE ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.ONE ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.TWO ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.TWO ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.TWO ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.THREE ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.THREE ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.THREE ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.FOUR ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.FOUR ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.FOUR ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.FIVE ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.FIVE ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.FIVE ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.SIX ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.SIX ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.SIX ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.IMPOSTER ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.IMPOSTER ) );
			_model.SandDeck.Add( new Card( CardSuit.SAND, CardType.IMPOSTER ) );

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

	}
}
