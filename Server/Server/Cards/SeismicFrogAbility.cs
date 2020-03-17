using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class SeismicFrogAbility : CardAbility
    {

        public const int damageDealt = 1;

        public SeismicFrogAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
         * Deal damage to all opponent's cards
         */ 
        public override void OnFieldTrigger()
        {

            Bounce();
            foreach (Card card in player.otherPlayer.deck.field.ToList())
            {

                card.DealDamage(damageDealt, false);

            }
        }

    }
}
