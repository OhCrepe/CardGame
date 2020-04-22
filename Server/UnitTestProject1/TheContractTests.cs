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
    public class TheContractTests
    {

        /*
         * Testing The Contract - the player should have both cards from their deck in their hand
         * (one from The Contract, one from drawing for turn)
         */
        [TestMethod]
        public void TestTheContractWhenValidTarget()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();
            game.player2.deck.lord = game.player2.deck.CreateCard("The Contract");

            game.player2.deck.deck.Add(game.player2.deck.CreateCard("Negotiator"));
            game.player2.deck.deck.Add(game.player2.deck.CreateCard("Assassin"));
            game.player2.deck.deck.Add(game.player2.deck.CreateCard("Negotiator"));
            game.player2.deck.deck.Add(game.player2.deck.CreateCard("Negotiator"));

            game.EndEffectsPhase();

            Assert.AreEqual(game.player2.deck.hand.Count, 2); // Indicates that we searched for "Assassin"
            Assert.AreEqual(game.player2.deck.deck.Count, 2);

            bool assassinFound = false;
            foreach(Card card in game.player2.deck.hand)
            {
                if (card.name == "Assassin") assassinFound = true;
            }

            Assert.AreEqual(assassinFound, true);

        }

        /*
         * Testing The Contract - the player should have both cards from their deck in their hand
         * (one from The Contract, one from drawing for turn)
         */
        [TestMethod]
        public void TestTheContractWhenNoValidTarget()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();
            game.player2.deck.lord = game.player2.deck.CreateCard("The Contract");

            game.player2.deck.deck.Add(game.player2.deck.CreateCard("Negotiator"));
            game.player2.deck.deck.Add(game.player2.deck.CreateCard("Negotiator"));
            game.player2.deck.deck.Add(game.player2.deck.CreateCard("Negotiator"));

            game.EndEffectsPhase();

            Assert.AreEqual(game.player2.deck.hand.Count, 1); // Indicates that we didn't search for "Assassin"
            Assert.AreEqual(game.player2.deck.deck.Count, 2);

            bool assassinFound = false;
            foreach (Card card in game.player2.deck.hand)
            {
                if (card.name == "Assassin") assassinFound = true;
            }

            Assert.AreNotEqual(assassinFound, true);

        }

    }

}
