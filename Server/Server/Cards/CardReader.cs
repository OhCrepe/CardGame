using Server.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class CardReader
    {

        public static SortedDictionary<string, Card> cardStats;

        /*
        *   Get the card stats and put them in a dictionary
        */
        public static void StartReader() {

            cardStats = new SortedDictionary<string, Card>();
            string filepath = Resources.CardData;
            string[] cards = File.ReadAllLines(filepath);
            for(int i = 1; i < cards.Length; i++)
            {

                string[] stats = cards[i].Split(',');
                Card card = new Card();
                card.name = stats[0];

                switch (stats[1])
                {

                    case "MINION":
                        card.cardType = Card.Type.MINION;
                        break;

                    case "UTILITY":
                        card.cardType = Card.Type.UTILITY;
                        break;

                    case "LORD":
                        card.cardType = Card.Type.LORD;
                        break;

                    default:
                        Console.WriteLine("Invalid card type");
                        continue;
                        
                }

                card.cost = int.Parse(stats[2]);
                card.health = int.Parse(stats[3]);
                card.strength = int.Parse(stats[4]);
                card.wageMod = int.Parse(stats[5]);
                card.wageBonus = int.Parse(stats[6]);

                cardStats.Add(card.name, card);

            }

        }


    }
}
