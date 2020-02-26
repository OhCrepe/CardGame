using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class TrinketsAndBaublesAbility : CardAbility
    {

        private const int goldGained = 5;

        public TrinketsAndBaublesAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
        * The ability of this card that triggers on summon
        * In this case, we give the player who played it 2 gold
        */
        public override void OnHire()
        {
            player.SetGold(player.gold + goldGained);
            CheckUtility();
        }

        /*
        * Validate that the target of this ability is correct
        */
        public override bool ValidateTarget(Card card)
        {
            return true;
        }

    }
}
