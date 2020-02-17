using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class GameState
    {

        public ServerDeckList player1Deck;
        public PlayerHandler player1, player2;

        public static bool targetting = false, attacking = false, deciding = false, dragging = false;
        public static string targettingCard = null, decidingCard = null;

        public enum Phase { START, DEBT, GOLD, START_EFFECTS, DRAW, MAIN, END_EFFECTS, END };
        public static Phase currentPhase;

        public PlayerHandler currentPlayer;

        private const int startingGold = 25;
        private const int goldPerTurn = 5;

        public GameState(PlayerHandler p1)
        {
            player1 = p1;
            
        }

        /*
        *   Initialize the Game state
        */
        public void InitializeGame()
        {
            currentPlayer = player1;
            player1Deck.ShuffleDeck();
            for (int i = 0; i < 4; i++)
            {
                player1Deck.DrawCard();
            }
            player1.SetGold(startingGold);
            MainPhase();
        }


        /*
        *	Logic that handles the start of a player's turn
        */
        public void StartPhase()
        {
            currentPhase = Phase.START;
            DebtPhase();
        }

        /*
        *	Logic that handles checking for a player's debt
        * Give them a debt counter if no gold left, or they lose the game if they have
        * no gold AND a debt counter
        */
        public void DebtPhase()
        {
            currentPhase = Phase.DEBT;
            GoldPhase();
        }

        /*
        *	Logic that handles the player's gold gained for the turn.
        * DEFUALT: Give 5 gold.
        */
        public void GoldPhase()
        {
            currentPhase = Phase.GOLD;
            player1.SetGold(player1.gold += goldPerTurn);
            StartEffectsPhase();
        }

        /*
        * Resolve all effects that trigger at the beginning of a player's turn
        */
        public void StartEffectsPhase()
        {
            currentPhase = Phase.START_EFFECTS;
            DrawPhase();
        }

        /*
        *	Turn player draws a card
        */
        public void DrawPhase()
        {
            currentPhase = Phase.DRAW;
            player1Deck.DrawCard();
            MainPhase();
        }

        /*
        *	In this phase the turn player gets to play their cards, hire units
        * and enter combat
        */
        public void MainPhase()
        {
            currentPhase = Phase.MAIN;
        }

        /*
        *	Resolve all effects that trigger at the end of a player's turn
        */
        public void EndEffectsPhase()
        {

            EndPhase();

        }

        /*
        *	End the current player's turn, give control to their opponent
        */
        public void EndPhase()
        {
            currentPhase = Phase.END;
            StartPhase();
        }

        /*
        *   Validate that the turn player can legally end their turn
        */
        public bool ValidateEndMainPhase()
        {

            if (currentPhase != Phase.MAIN) return false;

            return true;

        }

    }

}
