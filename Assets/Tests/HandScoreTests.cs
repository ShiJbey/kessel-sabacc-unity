using System.Collections.Generic;
using KesselSabacc.Model;
using NUnit.Framework;

public class HandScoreTests
{
	// A Test behaves as an ordinary method
	[Test]
	public void TestHasSabaccHand()
	{
		var p1 = new Hand();

		p1.Add( new Card( CardSuit.SAND, CardType.ONE ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.ONE ) );

		Assert.True( p1.HasSabacc() );
		p1.Clear();

		p1.Add( new Card( CardSuit.SAND, CardType.SIX ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.SIX ) );

		Assert.True( p1.HasSabacc() );
		p1.Clear();
	}

	[Test]
	public void TestHasSabaccHandSylop()
	{
		// Ensure that sylop + any card results in a sabacc hand
		var p1 = new Hand();

		p1.Add( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.IMPOSTER ).WithValue( (int)CardType.FOUR ) );

		Assert.True( p1.HasSabacc() );
		p1.Clear();

		p1.Add( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.THREE ) );

		Assert.True( p1.HasSabacc() );
		p1.Clear();
	}

	[Test]
	public void TestHasSabaccHandImposter()
	{
		// Ensure that imposter cards with overwritten values
		var p1 = new Hand();

		p1.Add( new Card( CardSuit.SAND, CardType.FIVE ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.IMPOSTER ) );

		Assert.False( p1.HasSabacc() );
		p1.Clear();
	}

	[Test]
	public void TestHasSabaccHandImposterWithValue()
	{
		// Ensure that imposter cards with overwritten values
		var p1 = new Hand();

		p1.Add( new Card( CardSuit.SAND, CardType.TWO ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.IMPOSTER ).WithValue( (int)CardType.TWO ) );

		Assert.True( p1.HasSabacc() );
		p1.Clear();
	}

	[Test]
	public void TestSabaccHandImposterAndSylop()
	{
		// Ensure that imposter cards, regardless of value, result in a sabacc hand
		// when paired with a sylop
		var p1 = new Hand();

		p1.Add( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.IMPOSTER ) );

		Assert.True( p1.HasSabacc() );
		p1.Clear();

		p1.Add( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.IMPOSTER ).WithValue( (int)CardType.THREE ) );

		Assert.True( p1.HasSabacc() );
		p1.Clear();
	}

	[Test]
	public void TestHasSabaccHandFails()
	{
		var p1 = new Hand();

		p1.Add( new Card( CardSuit.SAND, CardType.ONE ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.TWO ) );

		Assert.False( p1.HasSabacc() );
		p1.Clear();

		p1.Add( new Card( CardSuit.SAND, CardType.ONE ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.IMPOSTER ).WithValue( (int)CardType.THREE ) );

		Assert.False( p1.HasSabacc() );
		p1.Clear();
	}

	[Test]
	public void TestHasPrimeSabaccHand()
	{
		var p1 = new Hand();

		p1.Add( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.SYLOP ) );

		Assert.True( p1.HasPrimeSabacc() );

		p1.Clear();
	}

	[Test]
	public void TestHasPrimeSabaccFails()
	{
		var p1 = new Hand();

		p1.Add( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.ONE ) );

		Assert.False( p1.HasPrimeSabacc() );

		p1.Clear();
	}

	[Test]
	public void TestPrimeSabaccBeatsSabacc()
	{
		IHandScoreStrategy handScorer = new StandardHandScoreStrategy();

		var p1 = new Hand();
		p1.Add( new Card( CardSuit.SAND, CardType.TWO ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.TWO ) );
		var p1_Result = handScorer.ScoreHand( p1 );

		var p2 = new Hand();
		p2.Add( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p2.Add( new Card( CardSuit.BLOOD, CardType.ONE ) );
		var p2_Result = handScorer.ScoreHand( p2 );

		Assert.True( p1_Result < p2_Result );
	}

	[Test]
	public void TestSabaccBeatsNormalHand()
	{
		IHandScoreStrategy handScorer = new StandardHandScoreStrategy();

		// Test that Sabacc and Prime Sabacc hands beat other hands
		var p1 = new Hand();
		p1.Add( new Card( CardSuit.SAND, CardType.SIX ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.SIX ) );
		var p1_Result = handScorer.ScoreHand( p1 );

		var p2 = new Hand();
		p2.Add( new Card( CardSuit.SAND, CardType.THREE ) );
		p2.Add( new Card( CardSuit.BLOOD, CardType.FOUR ) );
		var p2_Result = handScorer.ScoreHand( p2 );

		Assert.True( p1_Result > p2_Result );

		var p3 = new Hand();
		p3.Add( new Card( CardSuit.SAND, CardType.SYLOP ) );
		p3.Add( new Card( CardSuit.BLOOD, CardType.SYLOP ) );
		var p3_Result = handScorer.ScoreHand( p3 );

		var p4 = new Hand();
		p4.Add( new Card( CardSuit.SAND, CardType.FIVE ) );
		p4.Add( new Card( CardSuit.BLOOD, CardType.FOUR ) );
		var p4_Result = handScorer.ScoreHand( p4 );

		Assert.True( p3_Result > p4_Result );
	}

	[Test]
	public void TestLowerHandBeatsOtherHand()
	{
		IHandScoreStrategy handScorer = new StandardHandScoreStrategy();

		// Test that a hand with a smaller difference beats the other hand.
		// This is the most basic check.
		var p1 = new Hand();
		p1.Add( new Card( CardSuit.SAND, CardType.THREE ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.TWO ) );
		var p1_Result = handScorer.ScoreHand( p1 );

		var p2 = new Hand();
		p2.Add( new Card( CardSuit.SAND, CardType.ONE ) );
		p2.Add( new Card( CardSuit.BLOOD, CardType.FOUR ) );
		var p2_Result = handScorer.ScoreHand( p2 );

		Assert.True( p1_Result > p2_Result );

		p1 = new Hand();
		p1.Add( new Card( CardSuit.SAND, CardType.THREE ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.ONE ) );
		p1_Result = handScorer.ScoreHand( p1 );

		p2 = new Hand();
		p2.Add( new Card( CardSuit.SAND, CardType.THREE ) );
		p2.Add( new Card( CardSuit.BLOOD, CardType.FOUR ) );
		p2_Result = handScorer.ScoreHand( p2 );

		Assert.True( p2_Result > p1_Result );
	}

	[Test]
	public void TestSmallerHandBeatsSameDifference()
	{
		IHandScoreStrategy handScorer = new StandardHandScoreStrategy();

		// Test that a hand with a smaller overall size should have a better
		// overall score than another hand with the same card difference.
		var p1 = new Hand();
		p1.Add( new Card( CardSuit.SAND, CardType.THREE ) );
		p1.Add( new Card( CardSuit.BLOOD, CardType.TWO ) );
		var p1_Result = handScorer.ScoreHand( p1 );

		var p2 = new Hand();
		p2.Add( new Card( CardSuit.SAND, CardType.THREE ) );
		p2.Add( new Card( CardSuit.BLOOD, CardType.FOUR ) );
		var p2_Result = handScorer.ScoreHand( p2 );

		Assert.True( p1_Result > p2_Result );

		var p3 = new Hand();
		p3.Add( new Card( CardSuit.SAND, CardType.THREE ) );
		p3.Add( new Card( CardSuit.BLOOD, CardType.THREE ) );
		var p3_Result = handScorer.ScoreHand( p3 );

		var p4 = new Hand();
		p4.Add( new Card( CardSuit.SAND, CardType.FOUR ) );
		p4.Add( new Card( CardSuit.BLOOD, CardType.FOUR ) );
		var p4_Result = handScorer.ScoreHand( p4 );

		Assert.True( p3_Result > p4_Result );
	}
}
