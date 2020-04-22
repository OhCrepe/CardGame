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
    public class AssassinationTests
    {

        /*
         * Testing assassination when the activation of it is valid
         */
        [TestMethod]
        public void TestAssassinationOnHealthyUnit()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card target = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(target);
            Card card = game.player2.deck.CreateCard("Assassination");
            game.player2.deck.deck.Add(card);
            game.SummonUnit(target.id);
            game.EndEffectsPhase();

            Assert.AreEqual(game.player1.deck.field.Count, 1);

            if (game.ValidateSummon(card.id, game.player2)){
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.targetting, true);
            Assert.AreEqual(game.targettingCard, card);

            game.targettingCard.ability.OnTargetSelect(target);

            Assert.AreEqual(game.player1.deck.discard.Contains(target), true);
            Assert.AreEqual(game.player2.deck.discard.Contains(card), true);

        }

        /*
         * Testing assassination when the activation of it is invalid
         */
        [TestMethod]
        public void TestAssassinationOnDamagedUnit()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card target = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(target);
            Card card = game.player2.deck.CreateCard("Assassination");
            game.player2.deck.deck.Add(card);
            game.SummonUnit(target.id);
            game.EndEffectsPhase();

            Assert.AreEqual(game.player1.deck.field.Count, 1);
            target.DealDamage(2, false);


            Assert.AreNotEqual(game.ValidateSummon(card.id, game.player2), true);

            Assert.AreNotEqual(game.targetting, true);
            Assert.AreNotEqual(game.targettingCard, card);

            Assert.AreEqual(game.player2.deck.hand.Contains(card), true);


        }

        [TestMethod]
        public void TestAssassinationOnInvalidTarget()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card target = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(target);
            Card card = game.player2.deck.CreateCard("Assassination");
            game.player2.deck.deck.Add(card);
            Card invalidTarget = game.player1.deck.CreateCard("Swelling Flesh");
            game.player1.deck.hand.Add(invalidTarget);
            game.SummonUnit(target.id);
            game.SummonUnit(invalidTarget.id);
            game.EndEffectsPhase();

            Assert.AreEqual(game.player1.deck.field.Count, 2);
            invalidTarget.DealDamage(2, false);

            Assert.AreNotEqual(card.ability.ValidateTarget(invalidTarget), true);
            Assert.AreEqual(card.ability.ValidateTarget(target), true);

        }

    }

}
