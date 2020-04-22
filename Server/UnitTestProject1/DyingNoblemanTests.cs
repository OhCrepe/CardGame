using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using Server.Cards;

namespace Tests
{
    [TestClass]
    public class DyingNoblemanTests
    {

        /*
         * Test the dying nobleman
         */
        [TestMethod]
        public void TestDyingNobleman()
        {

            CardReader.StartReader();
            GameState game = GameStateTests.SetupGameState();

            Card card = game.player1.deck.GetCardByName("Dying Nobleman");
            GameStateTests.EnsureCardInPlayersHand(game.player1, card);
            game.SummonUnit(card.id);

            card.ability.Kill(false);

            Assert.AreEqual(game.player1.gold, GameState.startingGold + DyingNoblemanAbility.goldGained - card.cost);

        }

    }
}
