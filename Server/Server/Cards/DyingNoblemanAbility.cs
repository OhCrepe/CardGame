using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class DyingNoblemanAbility : CardAbility
    {

        public const int goldGained = 6;

        public DyingNoblemanAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        public override void OnKillAbility(bool combat)
        {
            player.SetGold(player.gold + goldGained);
        }

    }
}
