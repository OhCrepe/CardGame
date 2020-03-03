using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class TheExecutionerAbility : CardAbility
    {

        public TheExecutionerAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
         * At the end of the turn, kill everything with only 1 health remaining
         */ 
        public override void EndOfTurnAbility()
        {
            foreach(Card card in game.player1.deck.field.ToList())
            {
                if(card.currentHealth == 1)
                {
                    card.ability.Kill(false);
                }
            }
        }

    }
}
