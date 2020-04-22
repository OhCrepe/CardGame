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
    public class FrogElderTests
    {

        /*
         * Testing frog elder ability to make sure it works when valid targets exist
         */
        [TestMethod]
        public void TestFrogElderWhenValid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Frog Elder");
            game.player1.deck.hand.Add(card);

            Card target = game.player1.deck.CreateCard("Illusion Frog");
            game.player1.deck.deck.Add(target);

            game.SummonUnit(card.id);
            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }

            Assert.AreEqual(game.targetting, true);
            Assert.AreEqual(game.targettingCard, card);

            if (card.ability.ValidateTarget(target))
            {
                card.ability.OnTargetSelect(target);
            }

            Assert.AreEqual(game.player1.deck.hand.Contains(card), true);
            Assert.AreEqual(game.player1.deck.hand.Contains(target), true);

        }

        /*
        * Testing cannibals ability to make sure only valid targets can be selected
        */
        [TestMethod]
        public void TestFrogElderWhenInvalid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Frog Elder");
            game.player1.deck.hand.Add(card);

            Card target = game.player1.deck.CreateCard("Negotiator");
            game.player1.deck.deck.Add(target);

            game.SummonUnit(card.id);
            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }

            Assert.AreEqual(game.targetting, false);
            Assert.AreNotEqual(card.ability.ValidateTarget(card), true);
            Assert.AreEqual(game.player1.deck.field.Contains(card), true);
            Assert.AreEqual(game.player1.deck.deck.Contains(target), true);

        }

    }

}
