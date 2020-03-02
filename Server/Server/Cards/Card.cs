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
            this.currentHealth = card.health;
            this.currentStrength = card.strength;
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

            switch (name.ToUpper())
            {

                case "BLACK MARKET":
                    return new BlackMarketAbility(player, this, game);
                    break;

                case "BLOATED BODY":
                    return new BloatedBodyAbility(player, this, game);
                    break;

                case "COMBAT MEDIC":
                    return new CombatMedicAbility(player, this, game);
                    break;

                case "DYING NOBLEMAN":
                    return new DyingNoblemanAbility(player, this, game);
                    break;

                case "FROG ELDER":
                    return new FrogElderAbility(player, this, game);
                    break;

                case "ILLUSION FROG":
                    return new IllusionFrogAbility(player, this, game);
                    break;

                case "SLEIGHT OF HAND":
                    return new SleightOfHandAbility(player, this, game);
                    break;

                case "THE EXECUTIONER":
                    return new TheExecutionerAbility(player, this, game);
                    break;

                case "NEGOTIATOR":
                    return new NegotiatorAbility(player, this, game);
                    break;

                case "TRINKETS & BAUBLES":
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

        public void DealDamage(int damage, bool combat)
        {
            currentHealth -= damage;
            player.SendMessage("DAMAGE#" + id + "#" + damage + "#" + combat);
            if(currentHealth <= 0)
            {
                ability.Kill(combat);
            }
        }

        public void Heal(int healthGained)
        {
            currentHealth += healthGained;
            if (currentHealth > health)
            {
                currentHealth = health;
            }
            player.SendMessage("HEAL#" + id + "#" + healthGained);
        }

        public void Restore()
        {
            currentHealth = health;
            currentStrength = strength;
            player.SendMessage("RESTORE#" + id);
        }

    }
}
