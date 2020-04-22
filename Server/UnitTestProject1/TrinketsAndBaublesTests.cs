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
    public class TrinketsAndBaublesTests
    {

        /*
         * Testing Trinkets and Baubles - very simply gives us some gold
         */
        [TestMethod]
        public void TestTrinketsAndBaubles()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Trinkets & Baubles");

            game.player1.deck.hand.Add(card);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.player1.deck.discard.Contains(card), true);
            Assert.AreEqual(GameState.startingGold + TrinketsAndBaublesAbility.goldGained, game.player1.gold);

        }

    }

}
