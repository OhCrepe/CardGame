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
    public class IllusionFrogTests
    {

        /*
         * Testing IllusionFrog when killed by combat - Ensure it properly restores health and such
         */
        [TestMethod]
        public void TestIllusionFrogKilledByCombat()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Illusion Frog");
            game.player1.deck.hand.Add(card);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            card.ability.Kill(true); // true = killed by combat

            Assert.AreEqual(game.player1.deck.hand.Contains(card), true);
            Assert.AreEqual(card.health, card.currentHealth);
            Assert.AreEqual(card.strength, card.currentStrength);

        }

        /*
         * Testing Grave Robber when killed by combat
         */
        [TestMethod]
        public void TestIllusionFrogKilledByNotCombat()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Illusion Frog");
            game.player1.deck.hand.Add(card);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            card.ability.Kill(false); // true = killed by combat

            Assert.AreEqual(game.player1.deck.discard.Contains(card), true);
            Assert.AreEqual(card.health, card.currentHealth);
            Assert.AreEqual(card.strength, card.currentStrength);

        }

        /*
         * Test illusion frogs interaction with Engorgement. There was a bug I found prior where
         * illusion frog's original strength would be used in combat if it died (even if it was buffed).
         * This test tests for that.
         */ 
        [TestMethod]
        public void TestIllusionFrogWithEngorgement()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card attackTarget = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(attackTarget);

            Card card = game.player2.deck.CreateCard("Illusion Frog");
            game.player2.deck.hand.Add(card);

            Card engorgement = game.player2.deck.CreateCard("Engorgement");
            game.player2.deck.deck.Add(engorgement);

            game.SummonUnit(attackTarget.id);
            game.EndEffectsPhase();

            if (game.ValidateSummon(card.id, game.player2))
            {
                game.SummonUnit(card.id);
            }

            game.SummonUnit(engorgement.id);
            engorgement.ability.OnTargetSelect(card);

            int currentStrength = card.currentStrength;
            Assert.AreEqual(card.strength + EngorgementAbility.strengthGained, card.currentStrength);

            if (game.ValidAttack(card, attackTarget)){
                game.Attack(card, attackTarget);
            }

            Assert.AreEqual(game.player2.deck.hand.Contains(card), true);
            Assert.AreEqual(attackTarget.currentHealth, attackTarget.health - currentStrength);

        }

    }

}
