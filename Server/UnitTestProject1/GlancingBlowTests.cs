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
    public class GlancingBlowTests
    {

        /*
         * Testing frog elder ability to make sure it works when valid targets exist
         */
        [TestMethod]
        public void TestGlancingBlowWhenValid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player2.deck.CreateCard("Glancing Blow");
            game.player2.deck.deck.Add(card);

            Card target = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(target);

            game.SummonUnit(target.id);
            game.EndEffectsPhase();

            if (game.ValidateSummon(card.id, game.player2))
            {
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.targetting, true);
            Assert.AreEqual(game.targettingCard, card);

            if (card.ability.ValidateTarget(target))
            {
                card.ability.OnTargetSelect(target);
            }

            Assert.AreEqual(target.currentHealth, target.health - GlancingBlowAbility.damageDealt);
            Assert.AreEqual(game.player2.deck.discard.Contains(card), true);

        }

        /*
        * Testing cannibals ability to make sure only valid targets can be selected
        */
        [TestMethod]
        public void TestGlancingBlowWhenInvalid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Glancing Blow");
            game.player1.deck.hand.Add(card);

            if (game.ValidateSummon(card.id, game.player1)){
                game.SummonUnit(card.id);
            }
            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }

            Assert.AreEqual(game.targetting, false);
            Assert.AreEqual(game.player1.deck.hand.Contains(card), true);
            Assert.AreNotEqual(card.ability.ValidActivation(), true);

        }

    }

}
