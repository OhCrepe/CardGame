using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class TheDoctorAbility : CardAbility
    {

        private const int healAmount = 1;

        public TheDoctorAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
         * At the end of the turn, heal all your units for 1 hp
         */
        public override void EndOfTurnAbility()
        {
            if (game.currentPlayer != player) return;
            foreach(Card card in player.deck.field.ToList())
            {
                card.Heal(healAmount);
            }

        }


    }
}
