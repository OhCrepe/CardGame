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
    public class CannibalTests
    {

        /*
         * Testing cannibals ability to make sure only valid targets can be selected
         */
        [TestMethod]
        public void TestCannibal()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player2.deck.CreateCard("Cannibal");
            game.player2.deck.deck.Add(card);
            Card friendlyCard = game.player2.deck.CreateCard("Swarm of Rats");
            game.player2.deck.hand.Add(friendlyCard);
            Card enemyCard = game.player1.deck.CreateCard("Frog Elder");
            game.player1.deck.hand.Add(enemyCard);

            game.SummonUnit(enemyCard.id);
            game.EndEffectsPhase();

            if (game.ValidateSummon(card.id, game.player2))
            {
                game.SummonUnit(card.id);
            }
            game.SummonUnit(friendlyCard.id);

            card.DealDamage(2, false);

            card.ability.OnFieldTrigger();

            Assert.AreEqual(card.ability.ValidateTarget(enemyCard), false);

            if (card.ability.ValidateTarget(friendlyCard))
            {
                card.ability.OnTargetSelect(friendlyCard);
            }

            Assert.AreEqual(game.player2.deck.discard.Contains(friendlyCard), true);
            Assert.AreEqual(card.currentHealth, card.health);

        }

        /*
         * Testing cannibals ability to make sure the ability doesn't activate when there's no targets
         */
        [TestMethod]
        public void TestCannibalOnEmptyField()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Cannibal");
            game.player1.deck.hand.Add(card);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            card.DealDamage(2, false);

            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }

            Assert.AreEqual(card.ability.ValidActivation(), false);
            Assert.AreNotEqual(game.targetting, true);
            Assert.AreNotEqual(game.targettingCard, card);

        }

        /*
         * Testing cannibals ability to make sure it can't activate multiple times per turn
         */
        [TestMethod]
        public void TestCannibalOncePerTurn()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Cannibal");
            game.player1.deck.hand.Add(card);

            Card target1 = game.player1.deck.CreateCard("Illusion Frog");
            game.player1.deck.hand.Add(target1);

            Card target2 = game.player1.deck.CreateCard("Illusion Frog");
            game.player1.deck.hand.Add(target2);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }
            game.SummonUnit(target1.id);
            game.SummonUnit(target2.id);

            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }
            if (card.ability.ValidateTarget(target1))
            {
                card.ability.OnTargetSelect(target1);
            }

            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }
            if (game.targetting) { 
                if (game.targettingCard.id == card.id && card.ability.ValidateTarget(target2))
                {
                    card.ability.OnTargetSelect(target2);
                }
            }

            Assert.AreEqual(game.player1.deck.discard.Contains(target1), true);
            Assert.AreEqual(game.player1.deck.field.Contains(target2), true);
            Assert.AreEqual(card.health, card.currentHealth);
            Assert.AreEqual(card.ability.ValidActivation(), false);
            Assert.AreNotEqual(game.targetting, true);
            Assert.AreNotEqual(game.targettingCard, card);

        }


    }

}
