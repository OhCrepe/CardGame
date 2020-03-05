﻿using System;
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
            player.SendMessage("ID#" + lord.name + "#" + lord.id);
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

        /*
         * Shuffle the deck
         */
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
         * Shuffle the card into deck
         */
        public void ShuffleIntoDeck(Card card)
        {
            if (hand.Contains(card))
            {
                hand.Remove(card);
            } else if (field.Contains(card))
            {
                field.Remove(card);
            } else if (discard.Contains(card))
            {
                discard.Remove(card);
            }
            if (!deck.Contains(card))
            {
                deck.Add(card);
            }
            ShuffleDeck();
            player.SendMessage("PUTINDECK#" + card.id);
            player.otherPlayer.SendMessage("RFH");
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
        public void MoveCardToHand(Card card)
        {

            deck.Remove(card);
            hand.Add(card);
            player.SendMessage("ATH#" + card.id);
            player.otherPlayer.SendMessage("ATH_OPP");

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

        /*
         * Get the given card from our hand
         */
        public Card GetCardFromHand(string id)
        {
            foreach(Card card in hand)
            {
                if (card.id == id) return card;
            }
            return null;
        }

        /*
         * Pay unit wages
         */
        public void PayWages()
        {

            int wageMod = 1;
            int wageBonus = 0;
            int unitCount = 0;
            foreach (Card card in field)
            {
                wageMod *= card.wageMod;
                wageBonus += card.wageBonus;
                unitCount++;
            }
            int finalWages = (unitCount + wageBonus) * wageMod;
            player.SetGold(player.gold - finalWages);

        }

        /*
         * Check there are cards in deck with the given string
         */
        public bool CardInDeckWithString(string searchString)
        {
            foreach (Card card in deck)
            {
                if (card.name.Contains(searchString)) return true;
            }
            return false;
        }

    }
}