using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Cards;
using Server;
using Tests;

namespace Tests
{

    [TestClass]
    public class SwellingFleshTests
    {

        /*
         * Testing Rain of Fire - Make sure that we only heal 1 health per turn
         */
        [TestMethod]
        public void TestSwellingFleshWhenDamaged()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(card);
            game.SummonUnit(card.id);

            card.DealDamage(2, false);
            game.EndEffectsPhase();

            Assert.AreEqual(card.health - 1, card.currentHealth);
            Assert.AreEqual(card.strength + 1, card.currentStrength);

        }

        /*
         * Testing Swellish Flesh, we shouldn't heal when not damaged
         */
        [TestMethod]
        public void TestSwellingFleshWhenFullHealth()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(card);
            game.SummonUnit(card.id);

            game.EndEffectsPhase();

            Assert.AreEqual(card.health, card.currentHealth);
            Assert.AreEqual(card.strength + 1, card.currentStrength);

        }

        /*
         * Testing Swellish Flesh, we shouldn't during the opponent's turn
         */
        [TestMethod]
        public void TestSwellingFleshOnOpponentsTurn()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(card);
            game.SummonUnit(card.id);
            card.DealDamage(2, false);

            game.EndEffectsPhase();

            Assert.AreEqual(card.health - 1, card.currentHealth);
            Assert.AreEqual(card.strength + 1, card.currentStrength);

            game.EndEffectsPhase();

            Assert.AreEqual(card.health - 1, card.currentHealth);
            Assert.AreEqual(card.strength + 1, card.currentStrength);

        }

    }

}
