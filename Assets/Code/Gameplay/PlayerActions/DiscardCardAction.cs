using System.Collections;
using UnityEngine;
using KesselSabacc.Model;
using KesselSabacc.Views;

namespace KesselSabacc.Gameplay.PlayerActions
{
	/// <summary>
	/// The performer discards a duplicate card from their hand.
	/// </summary>
	public class DiscardCardAction : PlayerAction
	{
		private Card _card;

		public DiscardCardAction(PlayerController performer, Card card) : base( performer )
		{
			_card = card;
		}

		public override void ApplyToModel(KesselSabaccGameModel model)
		{

		}

		public override IEnumerator Execute(KesselSabaccGameController gameController)
		{
			yield return gameController.DiscardCardFromPlayer( Performer.PlayerIndex, _card, () =>
			{
				Debug.Log( $"{Performer.Model.Name} discarded {_card}." );
			} );
		}
	}
}
