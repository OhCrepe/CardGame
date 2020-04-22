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
    public class TheDoctorTests
    {

        /*
         * Testing The Doctor - both of player 1's units should have healed, however player 2's should not
         */
        [TestMethod]
        public void TestTheDoctor()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();
            game.player1.deck.lord = game.player1.deck.CreateCard("The Doctor");

            Card damagedCard = game.player1.deck.CreateCard("Seismic Frog");
            Card healthyCard = game.player1.deck.CreateCard("Seismic Frog");
            Card enemyCard = game.player2.deck.CreateCard("Seismic Frog");

            game.player1.deck.field.Add(damagedCard);
            game.player1.deck.field.Add(healthyCard);
            game.player2.deck.field.Add(enemyCard);

            damagedCard.DealDamage(2, false);
            enemyCard.DealDamage(2, false);

            game.EndEffectsPhase();

            Assert.AreEqual(healthyCard.health, healthyCard.currentHealth);
            Assert.AreEqual(damagedCard.health - 2 + TheDoctorAbility.healAmount, damagedCard.currentHealth);
            Assert.AreEqual(enemyCard.health - 2, enemyCard.currentHealth);

        }

    }

}
