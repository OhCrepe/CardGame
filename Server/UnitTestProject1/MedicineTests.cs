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
    public class MedicineTests
    {

        /*
         * Test that the card resolves properly when there's a valid target on field - fully healing it
         */ 
        [TestMethod]
        public void TestMedicineWhenValid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card target = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(target);
            Card card = game.player1.deck.CreateCard("Medicine");
            game.player1.deck.hand.Add(card);

            game.SummonUnit(target.id);
            target.DealDamage(3, false);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.targetting, true);
            Assert.AreEqual(game.targettingCard, card);

            if (card.ability.ValidateTarget(target)){
                card.ability.OnTargetSelect(target);
            }

            Assert.AreEqual(target.health, target.currentHealth);
            Assert.AreEqual(game.player1.deck.discard.Contains(card), true);

        }

        /*
         * Test that the card resolves properly when there's not a valid target on the field - that means not resolving
         */
        [TestMethod]
        public void TestMedicineWhenInvalidActivation()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Medicine");
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
