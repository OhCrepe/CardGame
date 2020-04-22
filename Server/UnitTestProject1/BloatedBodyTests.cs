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
    public class BloatedBodyTests
    {

        /*
         * Testing bloated body
         */
        [TestMethod]
        public void TestBloatedBody()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player2.deck.CreateCard("Bloated Body");
            game.player2.deck.deck.Add(card);
            Card friendlyCard = game.player2.deck.CreateCard("Swelling Flesh");
            Card enemyCard = game.player1.deck.CreateCard("Frog Elder");
            game.player2.deck.hand.Add(friendlyCard);
            game.player1.deck.hand.Add(enemyCard);

            game.SummonUnit(enemyCard.id);
            game.EndEffectsPhase();

            game.SummonUnit(friendlyCard.id);
            if (game.ValidateSummon(card.id, game.player2))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.player1.deck.field.Count, 1);
            Assert.AreEqual(game.player2.deck.field.Count, 2);

            game.Attack(card, enemyCard);

            Assert.AreEqual(game.player2.deck.discard.Contains(card), true);
            Assert.AreEqual(game.player1.deck.discard.Contains(enemyCard), true);

            Assert.AreEqual(friendlyCard.currentHealth, friendlyCard.health - BloatedBodyAbility.deathDamage);

        }

    }

}
