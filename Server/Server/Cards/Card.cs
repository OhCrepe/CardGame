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

        public CardAbility ability;

        public PlayerHandler player;
        public GameState game;

        public Card()
        {
            cardCount++;
        }

        public Card(Card card, PlayerHandler player, GameState game)
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

            this.player = player;
            this.game = game;

            ability = AssignAbility();

        }

        public CardAbility AssignAbility()
        {

            switch (name)
            {
                case "Black Market":
                    return new BlackMarketAbility(player, this, game);
                    break;

                case "Trinkets & Baubles":
                    return new TrinketsAndBaublesAbility(player, this, game);
                    break;

                default:
                    return null;
                    break;
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
