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
    public class EngorgementTests
    {

        /*
         * Test that the card resolves properly when there's a valid target on field
         */ 
        [TestMethod]
        public void TestEngorgementWhenValid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Engorgement");
            game.player1.deck.hand.Add(card);
            Card target = game.player1.deck.CreateCard("Negotiator");
            game.player1.deck.hand.Add(target);

            game.SummonUnit(target.id);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.targetting, true);
            Assert.AreEqual(game.targettingCard, card);

            if (card.ability.ValidateTarget(target)){
                card.ability.OnTargetSelect(target);
            }

            Assert.AreEqual(target.currentStrength, target.strength + EngorgementAbility.strengthGained);
            Assert.AreEqual(game.player1.deck.discard.Contains(card), true);


        }

        /*
         * Test that the card resolves properly when there's not a valid target on the field
         */
        [TestMethod]
        public void TestEngorgementWhenInvalidActivation()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Engorgement");
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
