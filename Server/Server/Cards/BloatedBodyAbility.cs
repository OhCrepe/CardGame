using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    public class BloatedBodyAbility : CardAbility
    {

        public const int deathDamage = 3;

        public BloatedBodyAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
        *   Upon dying, deal damage to every other unit on the field 
        */
        public override void OnKillAbility(bool combat)
        {

            if (player.deck.field.Contains(card))
            {
                return;
            }
            foreach(Card card in player.deck.field.ToList())
            {
                card.DealDamage(deathDamage, false);
            }
            foreach (Card card in player.otherPlayer.deck.field.ToList())
            {
                card.DealDamage(deathDamage, false);
            }

        }

    }
}
