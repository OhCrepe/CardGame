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
        public bool hasAttacked;

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

            this.id = name + cardCount;


            this.player = player;
            this.game = game;

            hasAttacked = false;
            ability = AssignAbility();

        }

        /*
         *  Assign the correct ability to our card based on its name
         */
        public CardAbility AssignAbility()
        {

            switch (name.ToUpper())
            {

                case "ASSASSIN":
                    return new AssassinAbility(player, this, game);
                    break;

                case "ASSASSINATION":
                    return new AssassinationAbility(player, this, game);
                    break;

                case "BANISHMENT":
                    return new BanishmentAbility(player, this, game);

                case "BLACK MARKET":
                    return new BlackMarketAbility(player, this, game);
                    break;

                case "BLOATED BODY":
                    return new BloatedBodyAbility(player, this, game);
                    break;

                case "CANNIBAL":
                    return new CannibalAbility(player, this, game);
                    break;

                case "COMBAT MEDIC":
                    return new CombatMedicAbility(player, this, game);
                    break;

                case "DYING NOBLEMAN":
                    return new DyingNoblemanAbility(player, this, game);
                    break;

                case "ENGORGEMENT":
                    return new EngorgementAbility(player, this, game);
                    break;

                case "FROG ELDER":
                    return new FrogElderAbility(player, this, game);
                    break;

                case "GLANCING BLOW":
                    return new GlancingBlowAbility(player, this, game);
                    break;

                case "GRAVE ROBBER":
                    return new GraveRobberAbility(player, this, game);
                    break;

                case "ILLUSION FROG":
                    return new IllusionFrogAbility(player, this, game);
                    break;

                case "MEDICINE":
                    return new MedicineAbility(player, this, game);
                    break;

                case "NEGOTIATOR":
                    return new NegotiatorAbility(player, this, game);
                    break;

                case "RAIN OF FIRE":
                    return new RainOfFireAbility(player, this, game);
                    break;

                case "SEISMIC FROG":
                    return new SeismicFrogAbility(player, this, game);
                    break;

                case "SLEIGHT OF HAND":
                    return new SleightOfHandAbility(player, this, game);
                    break;

                case "SWARM OF RATS":
                    return new SwarmOfRatsAbility(player, this, game);
                    break;

                case "SWELLING FLESH":
                    return new SwellingFleshAbility(player, this, game);
                    break;

                case "THE CONTRACT":
                    return new TheContractAbility(player, this, game);
                    break;

                case "THE DOCTOR":
                    return new TheDoctorAbility(player, this, game);
                    break;

                case "THE EXECUTIONER":
                    return new TheExecutionerAbility(player, this, game);
                    break;

                case "THE FROG LORD":
                    return new TheFrogLordAbility(player, this, game);
                    break;

                case "TRINKETS & BAUBLES":
                    return new TrinketsAndBaublesAbility(player, this, game);
                    break;

                default:
                    return null;
                    break;

            }

        }

        /*
         *  2 cards are equal if they share an id
         */
        public override bool Equals(Object obj)
        {
            Card card = (Card)obj;
            if (id == null || card.id == null) return false;
            return card.id == id;
        }

        /*
         * Deal damage to this card. Combat is true if it was done through an attack.
         */
        public void DealDamage(int damage, bool combat)
        {
            currentHealth -= damage;
            string message = "DAMAGE#" + id + "#" + damage;
            player.SendMessage(message);
            player.otherPlayer.SendMessage(message);
            if (currentHealth <= 0)
            {
                ability.Kill(combat);
            }
        }

        /*
         * Restore health to this card
         */
        public void Heal(int healthGained)
        {
            currentHealth += healthGained;
            if (currentHealth > health)
            {
                currentHealth = health;
            }
            string message = "HEAL#" + id + "#" + healthGained;
            player.SendMessage(message);
            player.otherPlayer.SendMessage(message);
        }

        public void IncreaseStrength(int strengthGained)
        {
            currentStrength += strengthGained;
            string message = "STRENGTH_UP#" + id + "#" + strengthGained;
            player.SendMessage(message);
            player.otherPlayer.SendMessage(message);
        }

        /*
         * Restore this card to full health and strength
         */
        public void Restore()
        {
            currentHealth = health;
            currentStrength = strength;
            player.SendMessage("RESTORE#" + id);
        }

        /*
         * Restore once per turns
         */
        public void ResetOncePerTurns()
        {
            ability.ResetOncePerTurn();
            hasAttacked = false;
            player.SendMessage("RESET_OPT#" + id);
        }

    }
}
