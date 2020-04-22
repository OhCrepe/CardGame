using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using Server.Cards;

namespace Tests
{
    [TestClass]
    public class GameStateTests
    {

        /*
         * Test that both players start with the correct amount of gold
         */ 
        [TestMethod]
        public void TestInitialGameStateGold()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();

            Assert.AreEqual(GameState.startingGold, game.player1.gold);
            Assert.AreEqual(GameState.startingGold, game.player2.gold);

        }

        /*
         * Test that both players start with the correct number of cards in hand
         */
        [TestMethod]
        public void TestInitialGameStateHandSize()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();

            Assert.AreEqual(GameState.startingHandSize, game.player1.deck.hand.Count);
            Assert.AreEqual(GameState.startingHandSize, game.player2.deck.hand.Count);

        }

        /*
         * Test that attack can not occur on the first turn
         */ 
        [TestMethod]
        public void TestFirstTurnAttacks()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();

            Assert.AreEqual(game.CheckCanAttack(), false);

        }

        /*
         * Test the turn 2 game phase to make sure it's the Main
         */
        [TestMethod]
        public void TestSecondTurnGamePhase()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();
            game.EndEffectsPhase();

            Assert.AreEqual(game.currentPhase, GameState.Phase.MAIN);

        }

        /*
         * Test the turn 2 gold to make sure player 2 has gained gold
         */
        [TestMethod]
        public void TestSecondTurnGold()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();
            game.EndEffectsPhase();

            Assert.AreEqual(game.player1.gold, GameState.startingGold);
            Assert.AreEqual(game.player2.gold, (GameState.startingGold + GameState.goldPerTurn));

        }

        /*
         * Test the turn 2 hand size to make sure player 2 has drawn a card
         */
        [TestMethod]
        public void TestSecondTurnHandSize()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();
            game.EndEffectsPhase();

            Assert.AreEqual(game.player1.deck.hand.Count, GameState.startingHandSize);
            Assert.AreEqual(game.player2.deck.hand.Count, (GameState.startingHandSize + 1));

        }

        /*
         * Test summoning a unit
         */
        [TestMethod]
        public void TestSummonUnit()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();

            Card card = game.player1.deck.GetCardByName("Dying Nobleman");
            EnsureCardInPlayersHand(game.player1, card);

            if (game.ValidateSummon(card.id, game.currentPlayer)){
                game.SummonUnit(card.id);
            }

            Assert.AreEqual(game.player1.deck.field.Contains(card), true);
            Assert.AreEqual(game.player1.gold, GameState.startingGold - card.cost);

        }

        /*
         * Test summoning a unit
         */
        [TestMethod]
        public void TestSummonUnitWhenInvalid()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();

            Card card = game.player1.deck.GetCardByName("Swelling Flesh");
            EnsureCardInPlayersHand(game.player1, card);

            int gold = 2;
            game.player1.gold = gold;

            if (game.ValidateSummon(card.id, game.currentPlayer)){
                game.SummonUnit(card.id);
            }

            Assert.AreNotEqual(game.player1.deck.field.Contains(card), true);
            Assert.AreEqual(game.player1.gold, gold);

        }

        /*
         * Test attacking other units - in this case neither unit should die
         */
        [TestMethod]
        public void TestAttackingUnitsNoDeath()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();

            Card card = game.player1.deck.GetCardByName("Negotiator");
            EnsureCardInPlayersHand(game.player1, card);
            game.SummonUnit(card.id);

            game.EndEffectsPhase(); // Turn must end to allow player2 to summon

            Card card2 = game.player2.deck.GetCardByName("Negotiator");
            EnsureCardInPlayersHand(game.player2, card2);
            game.SummonUnit(card2.id);

            if (game.ValidAttack(card2, card)){
                game.Attack(card2, card);
            }

            Assert.AreEqual(game.player1.deck.field.Contains(card), true);
            Assert.AreEqual(game.player2.deck.field.Contains(card2), true);
            Assert.AreEqual(card.currentHealth, card.health - card2.strength);
            Assert.AreEqual(card2.currentHealth, card2.health - card.strength);

        }

        /*
         * Test attacking other units - in this case player 1's unit should die
         */
        [TestMethod]
        public void TestAttackingUnitsOneDeath()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();

            Card card = game.player1.deck.GetCardByName("Negotiator");
            EnsureCardInPlayersHand(game.player1, card);
            game.SummonUnit(card.id);

            game.EndEffectsPhase(); // Turn must end to allow player2 to summon

            Card card2 = game.player2.deck.GetCardByName("Swelling Flesh");
            EnsureCardInPlayersHand(game.player2, card2);
            game.SummonUnit(card2.id);

            if (game.ValidAttack(card2, card)){
                game.Attack(card2, card);
            }

            Assert.AreEqual(game.player1.deck.discard.Contains(card), true);
            Assert.AreEqual(game.player2.deck.field.Contains(card2), true);
            Assert.AreEqual(card2.currentHealth, card2.health - card.strength);

        }

        /*
         * Test attacking other units - in this case player 1's unit should die
         */
        [TestMethod]
        public void TestAttackingUnitsBothDeath()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();

            Card card = game.player1.deck.GetCardByName("Swarm of Rats");
            EnsureCardInPlayersHand(game.player1, card);
            game.SummonUnit(card.id);

            game.EndEffectsPhase(); // Turn must end to allow player2 to summon

            Card card2 = game.player2.deck.GetCardByName("Swarm of Rats");
            EnsureCardInPlayersHand(game.player2, card2);
            game.SummonUnit(card2.id);

            if (game.ValidAttack(card2, card)){
                game.Attack(card2, card);
            }

            Assert.AreEqual(game.player1.deck.discard.Contains(card), true);
            Assert.AreEqual(game.player2.deck.discard.Contains(card2), true);

        }

        /*
         * Test pillaging - player 1 must have no cards on field when player 2 attacks
         * Player 1 should lose gold equal to player 2 unit's strength
         */
        [TestMethod]
        public void TestPillaging()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();
            game.EndEffectsPhase();

            Card card = game.player2.deck.GetCardByName("Dying Nobleman");
            EnsureCardInPlayersHand(game.player2, card);
            game.SummonUnit(card.id);

            if (game.ValidDirectAttack(card))
            {
                game.DirectAttack(card);
            }

        }

        /*
         * Test to make sure that the game catches invalid attacks
         */
        [TestMethod]
        public void TestAttackValidations()
        {

            CardReader.StartReader();
            GameState game = SetupGameState();

            Card card = game.player1.deck.GetCardByName("Swarm of Rats");

            Assert.AreNotEqual(game.ValidDirectAttack(card), true);

            EnsureCardInPlayersHand(game.player1, card);
            game.SummonUnit(card.id);

            Assert.AreNotEqual(game.ValidDirectAttack(card), true);

            game.EndEffectsPhase();

            Card card2 = game.player2.deck.GetCardByName("Dying Nobleman");
            EnsureCardInPlayersHand(game.player2, card2);
            game.SummonUnit(card2.id);

            Assert.AreNotEqual(game.ValidDirectAttack(card2), true);
            Assert.AreNotEqual(game.ValidAttack(card, card2), true);
            Assert.AreEqual(game.ValidAttack(card2, card), true);

            game.Attack(card2, card);

            Assert.AreNotEqual(game.ValidAttack(card2, card), true);

        }

        /*
         * Setup a blank gamestate for testing
         */
        public static GameState SetupGameState()
        {

            GameState game = new GameState(new PlayerHandler(null));
            game.player2 = new PlayerHandler(null);
            game.player2.game = game;
            game.InitializeGame();
            return game;

        }

        /*
         * Setup a blank gamestate for testing
         */
        public static GameState SetupEmptyGameState()
        {

            GameState game = new GameState(new PlayerHandler(null));
            game.player2 = new PlayerHandler(null);
            game.player2.game = game;
            game.InitializeGame();
            while(game.player1.deck.hand.Count > 0)
            {
                game.player1.deck.ShuffleIntoDeck(game.player1.deck.hand[0]);
            }
            game.player1.deck.deck.Clear();
            while (game.player2.deck.hand.Count > 0)
            {
                game.player2.deck.ShuffleIntoDeck(game.player2.deck.hand[0]);
            }
            game.player2.deck.deck.Clear();
            return game;

        }

        /*
         * If the card is not in the given players hand, add it to their hand (if it is in the deck)
         */
        public static void EnsureCardInPlayersHand(PlayerHandler player, Card card)
        {
            if (player.deck.deck.Contains(card))
            {
                player.deck.MoveCardToHand(card);
            }
        }



    }
}
