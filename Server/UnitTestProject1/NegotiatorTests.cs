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
    public class NegotiatorTests
    {

        /*
         * Testing Negotiator - The player's gold shouldn't change after they end their turn
         */
        [TestMethod]
        public void TestNegotiator()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Negotiator");
            game.player1.deck.hand.Add(card);
            Card otherCard = game.player1.deck.CreateCard("Illusion Frog");
            game.player1.deck.hand.Add(otherCard);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }
            game.SummonUnit(otherCard.id);

            int goldAtEndOfTurn = game.player1.gold;
            game.EndEffectsPhase();

            Assert.AreEqual(game.player1.deck.field.Contains(card), true);
            Assert.AreEqual(game.player1.gold, goldAtEndOfTurn); // Indicates that we haven't paid wages

        }

    }

}
