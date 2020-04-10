using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class BloodsuckerAbility : CardAbility
    {

        public const int deathDamage = 3;

        public BloodsuckerAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
        *   Upon dying, deal damage to every other unit on the field 
        */
        public override void OnAttackAbility(int damage, bool combat)
        {

            card.Heal(card.health - card.currentHealth);

        }

    }
}
