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
    public class SeismicFrogTests
    {

        /*
         * Testing Seismic Frog - Make sure that 1 damage is dealt to all units on enemies field - and that it doesn't damage ours
         */
        [TestMethod]
        public void TestSeismicFrog()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card enemyCard1 = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(enemyCard1);
            Card enemyCard2 = game.player1.deck.CreateCard("Illusion Frog");
            game.player1.deck.hand.Add(enemyCard2);
            Card enemyCard3 = game.player1.deck.CreateCard("Dying Nobleman");
            game.player1.deck.hand.Add(enemyCard3);

            game.SummonUnit(enemyCard1.id);
            game.SummonUnit(enemyCard2.id);
            game.SummonUnit(enemyCard3.id);

            Card card = game.player2.deck.CreateCard("Seismic Frog");
            game.player2.deck.deck.Add(card);
            Card friendlyCard = game.player2.deck.CreateCard("Swelling Flesh");
            game.player2.deck.hand.Add(friendlyCard);

            game.EndEffectsPhase();

            game.SummonUnit(friendlyCard.id);
            if (game.ValidateSummon(card.id, game.player2))
            {
                game.SummonUnit(card.id);
            }
            card.ability.OnFieldTrigger();

            Assert.AreEqual(game.player2.deck.hand.Contains(card), true);
            Assert.AreEqual(game.player1.deck.field.Contains(enemyCard1), true);
            Assert.AreEqual(enemyCard1.currentHealth, enemyCard1.health - SeismicFrogAbility.damageDealt);
            Assert.AreEqual(enemyCard2.currentHealth, enemyCard2.health - SeismicFrogAbility.damageDealt);
            Assert.AreEqual(game.player1.deck.field.Contains(enemyCard2), true);
            Assert.AreEqual(game.player1.deck.discard.Contains(enemyCard3), true);
            Assert.AreEqual(friendlyCard.currentHealth, friendlyCard.health);

        }

        /*
         * Testing Seismic Frog, we should still be able to activate its ability it if our opponent controls no cards
         */
        [TestMethod]
        public void TestSeismicFrogOnEmptyField()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Seismic Frog");
            game.player1.deck.hand.Add(card);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }
            card.ability.OnFieldTrigger();

            Assert.AreEqual(game.player1.deck.hand.Contains(card), true);

        }

    }

}
