using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{


    class ServerDeckList
    {

        public string lord;
        public string[] deck;

        public ServerDeckList(string lord, string[] deck)
        {
            this.lord = lord;
            this.deck = deck;
        }

        /*
         * Print the list of cards in this deck
         */
        public void PrintDeckList()
        {
            Console.WriteLine("-- Deck --");
            Console.WriteLine(lord + "");
            for (int i = 0; i < 25; i++)
            {
                Console.WriteLine(deck[i] + "");
            }
        }

    }
}
