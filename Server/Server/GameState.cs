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

        public GameState(PlayerHandler p1)
        {
            player1 = p1;
            
        }

        public void InitializeGame()
        {
            player1Deck.ShuffleDeck();
            for (int i = 0; i < 4; i++)
            {
                player1Deck.DrawCard();
            }
        }

    }

}
