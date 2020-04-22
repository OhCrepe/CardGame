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
    public class TheFrogLordTests
    {

        /*
         * Testing The Frog Lord - when seismic frog uses it's ability player 1 should draw a card
         */
        [TestMethod]
        public void TestTheFrogLord()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();
            game.player1.deck.lord = game.player1.deck.CreateCard("The Frog Lord");

            Card card = game.player1.deck.CreateCard("Seismic Frog");

            game.player1.deck.field.Add(card);
            game.player1.deck.deck.Add(game.player1.deck.CreateCard("Seismic Frog"));
            game.player1.deck.deck.Add(game.player1.deck.CreateCard("Seismic Frog"));

            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }

            Assert.AreEqual(game.player1.deck.hand.Contains(card), true);
            Assert.AreEqual(game.player1.deck.hand.Count, 2);
            Assert.AreEqual(game.player1.deck.deck.Count, 1);

        }

        /*
         * Testing The Frog Lord - should only draw when a card bounces to that's player hand, 
         * so if seismic frog uses it's ability player 2 should not draw a card
         */
        [TestMethod]
        public void TestTheFrogLordWhenEnemyBounces()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();
            game.player2.deck.lord = game.player2.deck.CreateCard("The Frog Lord");

            Card card = game.player1.deck.CreateCard("Seismic Frog");

            game.player1.deck.field.Add(card);
            game.player2.deck.deck.Add(game.player2.deck.CreateCard("Seismic Frog"));
            game.player2.deck.deck.Add(game.player2.deck.CreateCard("Seismic Frog"));

            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }

            Assert.AreEqual(game.player1.deck.hand.Contains(card), true);
            Assert.AreEqual(game.player2.deck.hand.Count, 0);
            Assert.AreEqual(game.player2.deck.deck.Count, 2);

        }

        /*
         * Testing The Frog Lord - when seismic frog uses it's ability player 1 should draw a card, but only once per turn
         */
        [TestMethod]
        public void TestTheFrogLordOncePerTurn()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();
            game.player1.deck.lord = game.player1.deck.CreateCard("The Frog Lord");

            Card card = game.player1.deck.CreateCard("Seismic Frog");

            game.player1.deck.field.Add(card);
            game.player1.deck.deck.Add(game.player1.deck.CreateCard("Seismic Frog"));
            game.player1.deck.deck.Add(game.player1.deck.CreateCard("Seismic Frog"));

            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }
            game.SummonUnit(card.id);
            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }

            Assert.AreEqual(game.player1.deck.hand.Contains(card), true);
            Assert.AreEqual(game.player1.deck.hand.Count, 2);
            Assert.AreEqual(game.player1.deck.deck.Count, 1);

        }

    }

}
