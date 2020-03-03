using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class IllusionFrogAbility : CardAbility
    {

        public IllusionFrogAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
         * Bounce instead of dying if by combat
         */ 
        public override void Kill(bool combat)
        {
            if (combat) Bounce();
            else base.Kill(combat);
        }

    }
}
