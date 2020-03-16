using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class RainOfFireAbility : CardAbility
    {

        private const int damageDealt = 2;

        public RainOfFireAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
        * The ability of this card that triggers on summon
        * In this case, we give the player who played it 5 gold
        */
        public override void OnHire()
        {
            foreach (Card card in player.otherPlayer.deck.field.ToList())
            {
                card.DealDamage(damageDealt, false);
            }
        }

    }
}
