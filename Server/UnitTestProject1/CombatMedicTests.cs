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
    public class CombatMedicTests
    {

        /*
         * Testing cannibals ability to make sure only valid targets can be selected
         */
        [TestMethod]
        public void TestCombatMedicWhenValid()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card target = game.player1.deck.CreateCard("Cannibal");
            game.player1.deck.hand.Add(target);
            Card card = game.player1.deck.CreateCard("Combat Medic");
            game.player1.deck.hand.Add(card);

            game.SummonUnit(target.id);
            int damage = 3;
            target.DealDamage(damage, false);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            int goldBeforeCost = game.player1.gold;

            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }
            if (card.ability.ValidateTarget(target))
            {
                card.ability.OnTargetSelect(target);
            }

            Assert.AreEqual(target.currentHealth, target.health + CombatMedicAbility.healAmount - damage);
            Assert.AreEqual(game.player1.gold, goldBeforeCost - CombatMedicAbility.abilityCost);

        }

        /*
         * Testing cannibals ability to make sure the ability doesn't activate when there's no targets
         */
        [TestMethod]
        public void TestCombatMedicOncePerTurn()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Combat Medic");
            game.player1.deck.hand.Add(card);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }
            if (card.ability.ValidateTarget(card))
            {
                card.ability.OnTargetSelect(card);
            }

            if (card.ability.ValidActivation())
            {
                card.ability.OnFieldTrigger();
            }

            Assert.AreEqual(game.player1.gold, GameState.startingGold - card.cost - CombatMedicAbility.abilityCost);
            Assert.AreEqual(card.ability.ValidActivation(), false);
            Assert.AreNotEqual(game.targetting, true);
            Assert.AreNotEqual(game.targettingCard, card);

        }


    }

}
