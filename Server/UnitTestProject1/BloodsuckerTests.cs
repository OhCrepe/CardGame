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
    public class BloodsuckerTests
    {

        /*
         * Test that blooduscker's ability works properly
         */
        [TestMethod]
        public void TestBloodsuckerSurviving()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player2.deck.CreateCard("Bloodsucker");
            game.player2.deck.deck.Add(card);
            Card enemyCard = game.player1.deck.CreateCard("Frog Elder");
            game.player1.deck.hand.Add(enemyCard);

            game.SummonUnit(enemyCard.id);
            game.EndEffectsPhase();

            if (game.ValidateSummon(card.id, game.player2))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.player1.deck.field.Count, 1);
            Assert.AreEqual(game.player2.deck.field.Count, 1);

            game.Attack(card, enemyCard);

            Assert.AreEqual(game.player2.deck.field.Contains(card), true);
            Assert.AreEqual(card.currentHealth, card.health);

        }

        /*
         * Test that blooduscker's ability doesn't prevent it from being killed
         */
        [TestMethod]
        public void TestBloodsuckerDying()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player2.deck.CreateCard("Bloodsucker");
            game.player2.deck.deck.Add(card);
            Card enemyCard = game.player1.deck.CreateCard("Bloodsucker");
            game.player1.deck.hand.Add(enemyCard);

            game.SummonUnit(enemyCard.id);
            game.EndEffectsPhase();

            if (game.ValidateSummon(card.id, game.player2))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.player1.deck.field.Count, 1);
            Assert.AreEqual(game.player2.deck.field.Count, 1);

            game.Attack(card, enemyCard);

            Assert.AreEqual(game.player2.deck.discard.Contains(card), true);

        }

        /*
         * Test that bloodsucker doesn't gain health when attacked
         */
        [TestMethod]
        public void TestBloodsuckerGettingAttacked()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player2.deck.CreateCard("Bloodsucker");
            game.player2.deck.deck.Add(card);
            Card enemyCard = game.player1.deck.CreateCard("Swarm of Rats");
            game.player1.deck.hand.Add(enemyCard);

            game.SummonUnit(enemyCard.id);
            game.EndEffectsPhase();

            if (game.ValidateSummon(card.id, game.player2))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.player1.deck.field.Count, 1);
            Assert.AreEqual(game.player2.deck.field.Count, 1);

            game.Attack(enemyCard, card);

            Assert.AreNotEqual(card.currentHealth, card.health);

        }

    }

}
