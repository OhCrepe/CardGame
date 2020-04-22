using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using Server.Cards;

namespace Tests
{
    [TestClass]
    public class ServerDeckListTests
    {

        /*
         * Test the turn 2 hand size to make sure player 2 has drawn a card
         */
        [TestMethod]
        public void TestDraw()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupGameState();
            game.player1.deck.DrawCard();

            Assert.AreEqual(game.player1.deck.hand.Count, GameState.startingHandSize + 1);

        }

        /*
        * Test the turn 2 hand size to make sure player 2 has drawn a card
        */
        [TestMethod]
        public void TestMoveCardToHand()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupGameState();
            Card card = game.player1.deck.deck[0];
            game.player1.deck.MoveCardToHand(card);

            Assert.AreEqual(game.player1.deck.hand.Contains(card), true);
            Assert.AreNotEqual(game.player1.deck.deck.Contains(card), true);
            Assert.AreEqual(game.player1.deck.hand.Count, GameState.startingHandSize + 1);

        }

        /*
        * Test the turn 2 hand size to make sure player 2 has drawn a card
        */
        [TestMethod]
        public void TestShuffleIntoDeck()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupGameState();
            Card card = game.player1.deck.hand[0];
            game.player1.deck.ShuffleIntoDeck(card);

            Assert.AreNotEqual(game.player1.deck.hand.Contains(card), true);
            Assert.AreEqual(game.player1.deck.deck.Contains(card), true);
            Assert.AreEqual(game.player1.deck.hand.Count, GameState.startingHandSize - 1);

        }

        /*
         * Test wages - ensure we're paying 1 gold per unit when we end our turn
         */
        [TestMethod]
        public void TestUnitWages()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupGameState();

            Card card = game.player1.deck.GetCardByName("Swarm of Rats");
            GameStateTests.EnsureCardInPlayersHand(game.player1, card);
            game.SummonUnit(card.id);

            card = game.player1.deck.GetCardByName("Swelling Flesh");
            GameStateTests.EnsureCardInPlayersHand(game.player1, card);
            game.SummonUnit(card.id);

            int beforeGold = game.player1.gold;
            int cardsOnField = game.player1.deck.field.Count;

            game.EndEffectsPhase();

            Assert.AreEqual(beforeGold - cardsOnField, game.player1.gold);
            Assert.AreEqual(cardsOnField, game.player1.deck.field.Count);

        }

    }
}
