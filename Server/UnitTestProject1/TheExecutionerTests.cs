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
    public class TheExecutionerTests
    {

        /*
         * Testing The Doctor - both of player 1's units should have healed, however player 2's should not
         */
        [TestMethod]
        public void TestTheExecutioner()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();
            game.player1.deck.lord = game.player1.deck.CreateCard("The Executioner");

            Card damagedCard = game.player1.deck.CreateCard("Seismic Frog");
            Card healthyCard = game.player1.deck.CreateCard("Seismic Frog");
            Card enemyCard = game.player2.deck.CreateCard("Seismic Frog");
            Card enemyLowHealthCard = game.player2.deck.CreateCard("Dying Nobleman");

            game.player1.deck.field.Add(damagedCard);
            game.player1.deck.field.Add(healthyCard);
            game.player2.deck.field.Add(enemyCard);
            game.player2.deck.field.Add(enemyLowHealthCard);

            damagedCard.currentHealth = 1;
            enemyCard.DealDamage(2, false);

            game.EndEffectsPhase();

            Assert.AreEqual(game.player1.deck.field.Contains(healthyCard), true);
            Assert.AreEqual(game.player2.deck.field.Contains(enemyCard), true);
            Assert.AreEqual(game.player1.deck.discard.Contains(damagedCard), true);
            Assert.AreEqual(game.player2.deck.discard.Contains(enemyLowHealthCard), true);

            Assert.AreEqual(healthyCard.health, healthyCard.currentHealth);
            Assert.AreEqual(enemyCard.health - 2, enemyCard.currentHealth);

        }

    }

}
