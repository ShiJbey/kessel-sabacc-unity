using System.Collections;
using UnityEngine;
using KesselSabacc.Model;

namespace KesselSabacc.Gameplay.PlayerActions
{
	/// <summary>
	/// The performer choses not to draw any cards this turn.
	/// </summary>
	public class StandAction : PlayerAction
	{
		public StandAction(PlayerController performer) : base( performer )
		{
		}

		public override void ApplyToModel(KesselSabaccGameModel model)
		{

		}

		public override IEnumerator Execute(KesselSabaccGameController gameController)
		{
			Debug.Log( $"{Performer.Model.Name} has stood." );
			Performer.Model.HasStoodThisTurn = true;
			Performer.IsTakingTurn = false;
			yield return null;
		}
	}
}
