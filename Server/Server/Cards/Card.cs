using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class Card
    {

        public static int cardCount;
        public string name;
        public int cost, strength, health;
        public enum Type { MINION, UTILITY, LORD };
        public Type cardType;
        public int wageMod, wageBonus;

        public int currentHealth, currentStrength;

        public readonly string id;

        public Card()
        {
            cardCount++;
        }

        public Card(Card card)
        {

            cardCount++;
            this.name = card.name;
            this.cost = card.cost;
            this.strength = card.strength;
            this.health = card.health;
            this.cardType = card.cardType;
            this.wageMod = card.wageMod;
            this.wageBonus = card.wageBonus;
            if (cardType == Type.LORD)
            {
                this.id = name;
            }
            else
            {
                this.id = name + cardCount;
            }

        }

        public override bool Equals(Object obj)
        {
            Card card = (Card)obj;
            if (id == null || card.id == null) return false;
            return card.id == id;
        }

    }
}
