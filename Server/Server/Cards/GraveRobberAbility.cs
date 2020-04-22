using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    public class GraveRobberAbility : CardAbility
    {

        public const int goldStolen = 2;

        public GraveRobberAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
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
            player.SetGold(player.gold += goldStolen);
            player.otherPlayer.SetGold(player.otherPlayer.gold -= goldStolen);

        }

    }
}
