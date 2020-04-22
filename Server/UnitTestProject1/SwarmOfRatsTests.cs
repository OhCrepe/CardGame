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
    public class SwarmOfRatsTests
    {

        /*
         * Test that the card resolves properly when there's a valid target in deck
         */ 
        [TestMethod]
        public void TestSwarmOfRatsAddWhenValid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            game.player1.deck.hand.Add(game.player1.deck.CreateCard("Swarm of Rats"));
            game.player1.deck.deck.Add(game.player1.deck.CreateCard("Swarm of Rats"));
            game.player1.deck.deck.Add(game.player1.deck.CreateCard("Medicine"));

            game.SummonUnit(game.player1.deck.hand[0].id);

            Assert.AreEqual(game.player1.deck.hand.Count, 1);
            Assert.AreEqual(game.player1.deck.hand[0].name, "Swarm of Rats");

        }

        /*
         * Test that the card resolves properly when there's not a valid target in deck
         */
        [TestMethod]
        public void TestSwarmOfRatsAddWhenInvalid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            game.player1.deck.hand.Add(game.player1.deck.CreateCard("Swarm of Rats"));
            game.player1.deck.deck.Add(game.player1.deck.CreateCard("Medicine"));

            game.SummonUnit(game.player1.deck.hand[0].id);

            Assert.AreEqual(game.player1.deck.hand.Count, 0);

        }

    }

}
