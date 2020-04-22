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
    public class BlackMarketTests
    {

        /*
         * Test that the card resolves properly when there's a valid target in hand
         */ 
        [TestMethod]
        public void TestBlackMarketWhenValid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Black Market");
            game.player1.deck.hand.Add(card);
            Card target = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(target);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.targetting, true);
            Assert.AreEqual(game.targettingCard, card);

            if (card.ability.ValidateTarget(target)){
                card.ability.OnTargetSelect(target);
            }

            Assert.AreEqual(game.player1.deck.hand.Count, 0);
            Assert.AreEqual(game.player1.gold, GameState.startingGold + target.cost);
            Assert.AreEqual(game.player1.deck.discard.Contains(target), true);

        }

        /*
         * Test that the card resolves properly when there's not a valid target in hand
         */
        [TestMethod]
        public void TestBlackMarketWhenInvalidActivation()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Black Market");
            game.player1.deck.hand.Add(card);

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
