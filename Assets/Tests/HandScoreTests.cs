using KesselSabacc.Gameplay;
using KesselSabacc.Model;
using NUnit.Framework;

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

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.SIX ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.SIX ) );

		Assert.True( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();
	}

	[Test]
	public void TestHasSabaccHandSylop()
	{
		// Ensure that sylop + any card results in a sabacc hand
		var p1 = new Player( "player", 0 );

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.IMPOSTER, (int)CardType.FOUR ) );

		Assert.True( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.THREE ) );

		Assert.True( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();
	}

	[Test]
	public void TestHasSabaccHandImposter()
	{
		// Ensure that imposter cards with overwritten values
		var p1 = new Player( "player", 0 );

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.FIVE ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.IMPOSTER ) );

		Assert.False( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();
	}

	[Test]
	public void TestHasSabaccHandImposterWithValue()
	{
		// Ensure that imposter cards with overwritten values
		var p1 = new Player( "player", 0 );

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.TWO ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.IMPOSTER, (int)CardType.TWO ) );

		Assert.True( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();
	}

	[Test]
	public void TestSabaccHandImposterAndSylop()
	{
		// Ensure that imposter cards, regardless of value, result in a sabacc hand
		// when paired with a sylop
		var p1 = new Player( "player", 0 );

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.IMPOSTER ) );

		Assert.True( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.IMPOSTER, (int)CardType.THREE ) );

		Assert.True( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();
	}

	[Test]
	public void TestHasSabaccHandFails()
	{
		var p1 = new Player( "player", 0 );

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.ONE ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.TWO ) );

		Assert.False( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.ONE ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.IMPOSTER, (int)CardType.THREE ) );

		Assert.False( HandScoreUtils.HasSabaccHand( p1 ) );
		p1.ClearHand();
	}

	[Test]
	public void TestHasPrimeSabaccHand()
	{
		var p1 = new Player( "player", 0 );

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.SYLOP ) );

		Assert.True( HandScoreUtils.HasPrimeSabaccHand( p1 ) );

		p1.ClearHand();
	}

	[Test]
	public void TestHasPrimeSabaccFails()
	{
		var p1 = new Player( "player", 0 );

		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.ONE ) );

		Assert.False( HandScoreUtils.HasPrimeSabaccHand( p1 ) );

		p1.ClearHand();
	}
}
