using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class SwellingFleshAbility : CardAbility
    {

        public SwellingFleshAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
         * At the end of the turn, heal 1 and buff strength 1
         */ 
        public override void EndOfTurnAbility()
        {
            if (game.currentPlayer != player) return;

            if (!player.deck.field.Contains(card)) return;
            card.Heal(1);
            card.IncreaseStrength(1);
        }

    }
}
