using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using Server.Cards;

namespace Tests
{
    [TestClass]
    public class CardTests
    {

        /*
         * Test to ensure taking damage works and can kill a unit
         */
        [TestMethod]
        public void TestDealDamage()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupGameState();

            Card card = game.player1.deck.GetCardByName("Swelling Flesh");
            GameStateTests.EnsureCardInPlayersHand(game.player1, card);
            game.SummonUnit(card.id);

            int damage = 2;
            card.DealDamage(damage, false);

            Assert.AreEqual(card.currentHealth, card.health - damage);
            Assert.AreEqual(game.player1.deck.field.Contains(card), true);

            card.DealDamage(card.currentHealth, false);

            Assert.AreEqual(card.currentHealth, card.health);
            Assert.AreEqual(game.player1.deck.discard.Contains(card), true);

        }

        /*
         * Test to ensure healing works as intended
         */
        [TestMethod]
        public void TestHeal()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupGameState();

            Card card = game.player1.deck.GetCardByName("Swelling Flesh");
            GameStateTests.EnsureCardInPlayersHand(game.player1, card);
            game.SummonUnit(card.id);

            int damage = 3;
            card.DealDamage(damage, false);
            Assert.AreEqual(card.currentHealth, card.health - damage);

            int heal = 1;
            card.Heal(heal);
            Assert.AreEqual(card.currentHealth, card.health - damage + heal);
            Assert.AreEqual(game.player1.deck.field.Contains(card), true);

        }

        /*
         * Test to ensure healing works as intended
         */
        [TestMethod]
        public void TestIncreaseStrength()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupGameState();

            Card card = game.player1.deck.GetCardByName("Swelling Flesh");
            GameStateTests.EnsureCardInPlayersHand(game.player1, card);
            game.SummonUnit(card.id);

            int strengthBuff = 3;
            card.IncreaseStrength(strengthBuff);
            Assert.AreEqual(card.currentStrength, card.strength + strengthBuff);

            int strengthNerf = -1;

            card.IncreaseStrength(strengthNerf);
            Assert.AreEqual(card.currentStrength, card.strength + strengthBuff + strengthNerf);

        }

        /*
         * Test to ensure restoring units work as intended
         */
        [TestMethod]
        public void TestRestore()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupGameState();

            Card card = game.player1.deck.GetCardByName("Swelling Flesh");
            GameStateTests.EnsureCardInPlayersHand(game.player1, card);
            game.SummonUnit(card.id);

            int strengthNerf = -1;
            card.IncreaseStrength(strengthNerf);

            int damage = 3;
            card.DealDamage(damage, false);

            Assert.AreNotEqual(card.currentStrength, card.strength);
            Assert.AreNotEqual(card.currentHealth, card.health);

            card.Restore();
            Assert.AreEqual(card.currentStrength, card.strength);
            Assert.AreEqual(card.currentHealth, card.health);

        }

    }
}
