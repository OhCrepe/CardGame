using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class TheFrogLordAbility : CardAbility
    {

        public TheFrogLordAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {
            isOncePerTurn = true;
        }

        /*
         * Once per turn when a unit bounces to our hand, we get to draw a card
         */
        public override void TriggerOnEvent(Trigger trigger, Card triggeringCard)
        {
            if (oncePerTurnUsed) return;
            if(trigger == Trigger.BOUNCE)
            {
                if (player.deck.hand.Contains(triggeringCard))
                {
                    player.deck.DrawCard();
                    oncePerTurnUsed = true;
                }
            }
        }

    }
}
