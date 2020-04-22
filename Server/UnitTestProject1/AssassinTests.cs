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
    public class AssassinTests
    {

        /*
         * Test that the card resolves properly when there's a valid target in deck
         */ 
        [TestMethod]
        public void TestAssassinAddWhenValid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            game.player1.deck.hand.Add(game.player1.deck.CreateCard("Assassin"));
            game.player1.deck.deck.Add(game.player1.deck.CreateCard("Assassination"));

            game.SummonUnit(game.player1.deck.hand[0].id);

            Assert.AreEqual(game.player1.deck.hand.Count, 1);
            Assert.AreEqual(game.player1.deck.hand[0].name, "Assassination");

        }

        /*
         * Test that the card resolves properly when there's not a valid target in deck
         */
        [TestMethod]
        public void TestAssassinAddWhenInvalid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            game.player1.deck.hand.Add(game.player1.deck.CreateCard("Assassin"));

            game.SummonUnit(game.player1.deck.hand[0].id);

            Assert.AreEqual(game.player1.deck.hand.Count, 0);

        }

    }

}
