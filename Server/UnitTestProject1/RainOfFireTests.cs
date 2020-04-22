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
    public class RainOfFireTests
    {

        /*
         * Testing Rain of Fire - Make sure that 2 damage is dealt to all units on enemies field - and that it doesn't damage ours
         */
        [TestMethod]
        public void TestRainOfFireOnField()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card enemyCard1 = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(enemyCard1);
            Card enemyCard2 = game.player1.deck.CreateCard("Illusion Frog");
            game.player1.deck.hand.Add(enemyCard2);
            Card enemyCard3 = game.player1.deck.CreateCard("Cannibal");
            game.player1.deck.hand.Add(enemyCard3);

            game.SummonUnit(enemyCard1.id);
            game.SummonUnit(enemyCard2.id);
            game.SummonUnit(enemyCard3.id);

            Card card = game.player2.deck.CreateCard("Rain of Fire");
            game.player2.deck.deck.Add(card);
            Card friendlyCard = game.player2.deck.CreateCard("Swelling Flesh");
            game.player2.deck.hand.Add(friendlyCard);

            game.EndEffectsPhase();

            game.SummonUnit(friendlyCard.id);
            if (game.ValidateSummon(card.id, game.player2))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.player2.deck.discard.Contains(card), true);
            Assert.AreEqual(game.player1.deck.field.Contains(enemyCard1), true);
            Assert.AreEqual(enemyCard1.currentHealth, enemyCard1.health - RainOfFireAbility.damageDealt);
            Assert.AreEqual(game.player1.deck.discard.Contains(enemyCard2), true);
            Assert.AreEqual(game.player1.deck.field.Contains(enemyCard3), true);
            Assert.AreEqual(enemyCard3.currentHealth, enemyCard3.health - RainOfFireAbility.damageDealt);
            Assert.AreEqual(friendlyCard.currentHealth, friendlyCard.health);

        }

        /*
         * Testing Rain of Fire, we should still be able to activate it if our opponent controls no cards
         */
        [TestMethod]
        public void TestRainOfFireOnEmptyField()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Rain of Fire");
            game.player1.deck.hand.Add(card);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.player1.deck.discard.Contains(card), true);

        }

    }

}
