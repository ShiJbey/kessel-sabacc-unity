using System.Collections;
using KesselSabacc.Model;
using KesselSabacc.Gameplay;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HandScoreTests
{
	// A Test behaves as an ordinary method
	[Test]
	public void TestHasSabaccHand()
	{
		var p1 = new Player( "player", 0 );
		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.ONE ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.ONE ) );

		Assert.True( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.ONE ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.TWO ) );

		Assert.False( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.ONE ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.IMPOSTER ) );

		Assert.False( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.ONE ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.IMPOSTER, (int)CardType.ONE ) );

		Assert.False( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();

	}
}
