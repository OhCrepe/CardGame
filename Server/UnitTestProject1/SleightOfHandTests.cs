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
    public class SleightOfHandTests
    {

        /*
         * Test that the card resolves properly when there's a valid target in hand and cards in the deck
         */ 
        [TestMethod]
        public void TestSleightOfHandWhenValid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Sleight of Hand");
            game.player1.deck.hand.Add(card);
            game.player1.deck.hand.Add(game.player1.deck.CreateCard("Medicine"));
            game.player1.deck.deck.Add(game.player1.deck.CreateCard("Medicine"));

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.targetting, true);
            Assert.AreEqual(game.targettingCard, card);

            Card target = game.player1.deck.hand[0];

            if (card.ability.ValidateTarget(target)){
                card.ability.OnTargetSelect(target);
            }

            Assert.AreEqual(game.player1.deck.hand.Count, 2);
            Assert.AreEqual(game.player1.deck.deck.Count, 0);
            Assert.AreEqual(game.player1.deck.discard.Contains(card), true);

        }

        /*
         * Test that the card resolves properly when there's not a valid target on the field - that means not resolving
         */
        [TestMethod]
        public void TestSleightOfHandWhenInvalidActivation()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Sleight of Hand");
            game.player1.deck.hand.Add(card);
            game.player1.deck.hand.Add(game.player1.deck.CreateCard("Medicine"));

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreNotEqual(game.ValidateSummon(card.id, game.player1), true);
            Assert.AreNotEqual(game.targetting, true);
            Assert.AreNotEqual(game.targettingCard, card);
            Assert.AreEqual(game.player1.deck.hand.Contains(card), true);

        }

    }

}
