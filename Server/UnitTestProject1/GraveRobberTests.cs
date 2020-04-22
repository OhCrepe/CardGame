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
    public class GraveRobberTests
    {

        /*
         * Testing Grave Robber when killed by combat
         */
        [TestMethod]
        public void TestGraveRobberKilledByCombat()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Grave Robber");
            game.player1.deck.hand.Add(card);

            game.SummonUnit(card.id);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            card.ability.Kill(true); // true = killed by combat

            Assert.AreEqual(game.player1.deck.discard.Contains(card), true);
            Assert.AreEqual(game.player1.gold, GameState.startingGold - card.cost + GraveRobberAbility.goldStolen);
            Assert.AreEqual(game.player2.gold, GameState.startingGold - GraveRobberAbility.goldStolen);


        }

        /*
         * Testing Grave Robber when killed by combat
         */
        [TestMethod]
        public void TestGraveRobberKilledByNotCombat()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupEmptyGameState();

            Card card = game.player1.deck.CreateCard("Grave Robber");
            game.player1.deck.hand.Add(card);

            game.SummonUnit(card.id);

            if (game.ValidateSummon(card.id, game.player1))
            {
                game.SummonUnit(card.id);
            }

            card.ability.Kill(false); // true = killed by combat

            Assert.AreEqual(game.player1.deck.discard.Contains(card), true);
            Assert.AreEqual(game.player1.gold, GameState.startingGold - card.cost + GraveRobberAbility.goldStolen);
            Assert.AreEqual(game.player2.gold, GameState.startingGold - GraveRobberAbility.goldStolen);


        }

    }

}
