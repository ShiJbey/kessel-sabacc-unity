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

	[Test]
	public void TestPrimeSabaccBeatsSabacc()
	{
		var p1 = new Player( "Player 1", 0 );
		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.TWO ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.TWO ) );
		var p1_Result = HandScoreUtils.CreateRoundResult( p1, 1 );

		var p2 = new Player( "Player 1", 0 );
		p2.AddCardToHand( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p2.AddCardToHand( new Card( CardSuit.BLOOD, CardType.ONE ) );
		var p2_Result = HandScoreUtils.CreateRoundResult( p2, 2 );

		Assert.AreEqual( p1_Result.HandDifference, p2_Result.HandDifference );
		Assert.True( p1_Result.PerformanceScore < p2_Result.PerformanceScore );
		Assert.True( p1_Result.HandSize > p2_Result.HandSize );
	}

	[Test]
	public void TestSabaccBeatsNormalHand()
	{
		// Test that Sabacc and Prime Sabacc hands beat other hands
		var p1 = new Player( "Player 1", 0 );
		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.SIX ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.SIX ) );
		var p1_Result = HandScoreUtils.CreateRoundResult( p1, 1 );

		var p2 = new Player( "Player 2", 0 );
		p2.AddCardToHand( new Card( CardSuit.SAND, CardType.THREE ) );
		p2.AddCardToHand( new Card( CardSuit.BLOOD, CardType.FOUR ) );
		var p2_Result = HandScoreUtils.CreateRoundResult( p2, 2 );

		Assert.True( p1_Result.HandDifference < p2_Result.HandDifference );
		Assert.True( p1_Result.PerformanceScore > p2_Result.PerformanceScore );
		Assert.True( p1_Result.HandSize > p2_Result.HandSize );

		var p3 = new Player( "Player 3", 0 );
		p3.AddCardToHand( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p3.AddCardToHand( new Card( CardSuit.BLOOD, CardType.SYLOP ) );
		var p3_Result = HandScoreUtils.CreateRoundResult( p3, 1 );

		var p4 = new Player( "Player 4", 0 );
		p4.AddCardToHand( new Card( CardSuit.SAND, CardType.FIVE ) );
		p4.AddCardToHand( new Card( CardSuit.BLOOD, CardType.FOUR ) );
		var p4_Result = HandScoreUtils.CreateRoundResult( p4, 2 );

		Assert.True( p3_Result.HandDifference < p4_Result.HandDifference );
		Assert.True( p3_Result.PerformanceScore > p4_Result.PerformanceScore );
		Assert.True( p3_Result.HandSize < p4_Result.HandSize );
	}

	[Test]
	public void TestLowerHandBeatsOtherHand()
	{
		// Test that a hand with a smaller difference beats the other hand.
		// This is the most basic check.
		var p1 = new Player( "Player 1", 0 );
		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.THREE ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.TWO ) );
		var p1_Result = HandScoreUtils.CreateRoundResult( p1, 1 );

		var p2 = new Player( "Player 2", 0 );
		p2.AddCardToHand( new Card( CardSuit.SAND, CardType.ONE ) );
		p2.AddCardToHand( new Card( CardSuit.BLOOD, CardType.FOUR ) );
		var p2_Result = HandScoreUtils.CreateRoundResult( p2, 2 );

		Assert.True( p1_Result.HandDifference < p2_Result.HandDifference );
		Assert.True( p1_Result.PerformanceScore > p2_Result.PerformanceScore );
		Assert.True( p1_Result.HandSize == p2_Result.HandSize );

		p1 = new Player( "Player 1", 0 );
		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.THREE ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.ONE ) );
		p1_Result = HandScoreUtils.CreateRoundResult( p1, 1 );

		p2 = new Player( "Player 2", 0 );
		p2.AddCardToHand( new Card( CardSuit.SAND, CardType.THREE ) );
		p2.AddCardToHand( new Card( CardSuit.BLOOD, CardType.FOUR ) );
		p2_Result = HandScoreUtils.CreateRoundResult( p2, 2 );


		Assert.AreEqual( p2_Result.CompareTo( p1_Result ), 1 );
		// Assert.True( p1_Result.HandDifference > p2_Result.HandDifference );
		// Assert.True( p1_Result.PerformanceScore < p2_Result.PerformanceScore );
		// Assert.True( p1_Result.HandSize < p2_Result.HandSize );
	}

	[Test]
	public void TestSmallerHandBeatsSameDifference()
	{
		// Test that a hand with a smaller overall size should have a better
		// overall score than another hand with the same card difference.
		var p1 = new Player( "Player 1", 0 );
		p1.AddCardToHand( new Card( CardSuit.SAND, CardType.THREE ) );
		p1.AddCardToHand( new Card( CardSuit.BLOOD, CardType.TWO ) );
		var p1_Result = HandScoreUtils.CreateRoundResult( p1, 1 );

		var p2 = new Player( "Player 2", 0 );
		p2.AddCardToHand( new Card( CardSuit.SAND, CardType.THREE ) );
		p2.AddCardToHand( new Card( CardSuit.BLOOD, CardType.FOUR ) );
		var p2_Result = HandScoreUtils.CreateRoundResult( p2, 2 );

		Assert.AreEqual( p1_Result.HandDifference, p2_Result.HandDifference );
		Assert.True( p1_Result.PerformanceScore > p2_Result.PerformanceScore );
		Assert.True( p1_Result.HandSize < p2_Result.HandSize );

		var p3 = new Player( "Player 3", 0 );
		p3.AddCardToHand( new Card( CardSuit.SAND, CardType.THREE ) );
		p3.AddCardToHand( new Card( CardSuit.BLOOD, CardType.THREE ) );
		var p3_Result = HandScoreUtils.CreateRoundResult( p3, 1 );

		var p4 = new Player( "Player 4", 0 );
		p4.AddCardToHand( new Card( CardSuit.SAND, CardType.FOUR ) );
		p4.AddCardToHand( new Card( CardSuit.BLOOD, CardType.FOUR ) );
		var p4_Result = HandScoreUtils.CreateRoundResult( p4, 2 );

		Assert.AreEqual( p3_Result.HandDifference, p4_Result.HandDifference );
		Assert.True( p3_Result.PerformanceScore > p4_Result.PerformanceScore );
		Assert.True( p3_Result.HandSize < p4_Result.HandSize );
	}
}
