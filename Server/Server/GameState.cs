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

        public GameState(PlayerHandler p1)
        {
            player1 = p1;
            
        }

        public void InitializeGame()
        {
            currentPlayer = player1;
            player1Deck.ShuffleDeck();
            for (int i = 0; i < 4; i++)
            {
                player1Deck.DrawCard();
            }
            MainPhase();
        }


        /*
        *	Logic that handles the start of a player's turn
        */
        public static void StartPhase()
        {
            currentPhase = Phase.START;
            DebtPhase();
        }

        /*
        *	Logic that handles checking for a player's debt
        * Give them a debt counter if no gold left, or they lose the game if they have
        * no gold AND a debt counter
        */
        public static void DebtPhase()
        {
            currentPhase = Phase.DEBT;
            GoldPhase();
        }

        /*
        *	Logic that handles the player's gold gained for the turn.
        * DEFUALT: Give 5 gold.
        */
        public static void GoldPhase()
        {
            currentPhase = Phase.GOLD;
            StartEffectsPhase();
        }

        /*
        * Resolve all effects that trigger at the beginning of a player's turn
        */
        public static void StartEffectsPhase()
        {
            currentPhase = Phase.START_EFFECTS;
            DrawPhase();
        }

        /*
        *	Turn player draws a card
        */
        public static void DrawPhase()
        {
            currentPhase = Phase.DRAW;
            MainPhase();
        }

        /*
        *	In this phase the turn player gets to play their cards, hire units
        * and enter combat
        */
        public static void MainPhase()
        {
            currentPhase = Phase.MAIN;
        }

        /*
        *	Resolve all effects that trigger at the end of a player's turn
        */
        public static void EndEffectsPhase()
        {

            EndPhase();

        }

        /*
        *	End the current player's turn, give control to their opponent
        */
        public static void EndPhase()
        {
            currentPhase = Phase.END;
            StartPhase();
        }

    }

}
