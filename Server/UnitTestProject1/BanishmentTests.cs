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
    public class BanishmentTests
    {

        /*
         * Test that the card resolves properly when there's a valid target on field
         */ 
        [TestMethod]
        public void TestBanishmentWhenValid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card target = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(target);
            Card card = game.player2.deck.CreateCard("Banishment");
            game.player2.deck.deck.Add(card);

            game.SummonUnit(target.id);
            game.EndEffectsPhase();

            if (game.ValidateSummon(card.id, game.player2))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.targetting, true);
            Assert.AreEqual(game.targettingCard, card);

            if (card.ability.ValidateTarget(target)){
                card.ability.OnTargetSelect(target);
            }

            Assert.AreEqual(game.player1.deck.deck.Contains(target), true);
            Assert.AreEqual(game.player2.deck.discard.Contains(card), true);


        }

        /*
         * Test that the card resolves properly when there's not a valid target on the field
         */
        [TestMethod]
        public void TestBanishmentWhenInvalidActivation()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Banishment");
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
