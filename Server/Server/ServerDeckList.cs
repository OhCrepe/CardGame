using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Server.Cards;

namespace Server
{

    class ServerDeckList
    {

        public Card lord;
        public List<Card> deck;
        public List<Card> hand;
        public List<Card> field;
        public List<Card> discard;
        public PlayerHandler player;

        private static readonly RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

        public ServerDeckList(PlayerHandler player, Card lord, Card[] deck)
        {
            this.player = player;
            this.lord = lord;
            this.deck = new List<Card>(deck);
            this.hand = new List<Card>();
            this.field = new List<Card>();
            this.discard = new List<Card>();
            foreach (Card card in this.deck)
            {
                player.SendMessage("ID#" + card.name + "#" + card.id);
            }
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

        public void ShuffleDeck()
        {
            List<Card> newDeck = new List<Card>();
            for (int i = deck.Count - 1; i >= 0; i--)
            {
                int cardIndex = (int) (GetNumberBetween(0, i));
                newDeck.Insert(0, deck[cardIndex]);
                deck.RemoveAt(cardIndex);
            }

            deck = newDeck;
        }

        /*
        * Draws a card from the top of the deck if the deck isn't empty
        */
        public Card DrawCard()
        {

            if (deck.Count == 0)
            {
                return null;
            }
            Card drawnCard = deck[0];
            MoveCardToHand(drawnCard);
            //CheckForDeck(); // Graphical and therefore doesn't matter so much
            return drawnCard;

        }

        /*
        *	Helper function to handle common functionality between searching and drawing
        */
        private void MoveCardToHand(Card card)
        {

            deck.Remove(card);
            hand.Add(card);
            player.SendMessage("ATH#" + card.id);

        }

        /*
         * Produces more random numbers than random.Range();
         * Source: https://scottlilly.com/create-better-random-numbers-in-c/
         */
        private int GetNumberBetween(int min, int max)
        {
            byte[] randomNumber = new byte[1];
            random.GetBytes(randomNumber);
            double value = Convert.ToDouble(randomNumber[0]);
            double multiplier = Math.Max(0, (value / 255d) - 0.00000000001d);
            int range = max - min + 1;
            double randomValueInRange = Math.Floor(multiplier * range);
            return (int)(min+randomValueInRange);
        }

        public Card GetCardFromHand(string id)
        {
            foreach(Card card in hand)
            {
                if (card.id == id) return card;
            }
            return null;
        }


    }
}
