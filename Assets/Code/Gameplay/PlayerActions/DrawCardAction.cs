using System.Collections;
using UnityEngine;
using KesselSabacc.Model;
using KesselSabacc.Views;

namespace KesselSabacc.Gameplay.PlayerActions
{
	/// <summary>
	/// The performer draws a single card from one of the four piles.
	/// </summary>
	public class DrawCardAction : PlayerAction
	{
		private CardStackView _cardStackView;
		private Card _card;

		public DrawCardAction(PlayerController performer, Card card, CardStackView cardStackView) : base( performer )
		{
			_cardStackView = cardStackView;
			_card = card;
		}

		public override void ApplyToModel(KesselSabaccGameModel model)
		{

		}

		public override IEnumerator Execute(KesselSabaccGameController gameController)
		{
			yield return gameController.DealCardToPlayer( _cardStackView, Performer.PlayerIndex, (cardView) =>
			{
				Performer.Model.Chips -= 1;
				Performer.Model.ChipsInvested += 1;
				Performer.Model.DrewCardThisTurn = true;
				Debug.Log( $"{Performer.Model.Name} drew {_card}." );
			} );
		}
	}
}
