using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using Server.Cards;

namespace Tests
{
    [TestClass]
    public class CardAbilityTests
    {

        /*
         * Test to ensure that killing cards produces the desired behavior
         */
        [TestMethod]
        public void TestCardKill()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupGameState();

            Card card = game.player1.deck.GetCardByName("Swelling Flesh");
            GameStateTests.EnsureCardInPlayersHand(game.player1, card);
            game.SummonUnit(card.id);
            Card card2 = game.player1.deck.deck[0];
            Card card3 = game.player1.deck.hand[0];

            Assert.AreEqual(game.player1.deck.field.Contains(card), true);

            card.ability.Kill(false);
            card2.ability.Kill(false);
            card3.ability.Kill(false);

            Assert.AreEqual(game.player1.deck.discard.Contains(card), true);
            Assert.AreEqual(game.player1.deck.discard.Contains(card2), true);
            Assert.AreEqual(game.player1.deck.discard.Contains(card3), true);

        }

        /*
        * Make sure cards bounce properly
        */
        [TestMethod]
        public void TestCardBouncing()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupGameState();
            Card card = game.player1.deck.hand[0];
            game.SummonUnit(card.id);

            Assert.AreEqual(game.player1.deck.field.Contains(card), true);

            card.ability.Bounce();

            Assert.AreEqual(game.player1.deck.hand.Contains(card), true);


        }

        /*
        * Make sure that once per turn abilities reset properly
        */
        [TestMethod]
        public void TestOncePerTurn()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupGameState();
            Card card = game.player1.deck.hand[0];

            card.ability.oncePerTurnUsed = true;
            card.ResetOncePerTurns();

            Assert.AreEqual(card.ability.oncePerTurnUsed, false);

        }

    }
}
