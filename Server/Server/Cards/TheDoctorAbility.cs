using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class TheDoctorAbility : CardAbility
    {

        public TheDoctorAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
        * Ensure this card is in hand and that there is a target for it's effect, and that there are cards to draw
        */
        public override bool ValidActivation()
        {
            return (player.deck.field.Count > 0);
        }

        /*
         * At the end of the turn, kill everything with only 1 health remaining
         */
        public override void EndOfTurnAbility()
        {
            if (!ValidActivation()) return;
            game.TargetCard(this);
        }

        /*
         *  What to do when a target is selected
         */
        public override void OnTargetSelect(Card card)
        {
            card.Heal(card.health);
            base.OnTargetSelect(card);
        }

        /*
         *  Validate that the target is a legal one
         */
        public override bool ValidateTarget(Card card)
        {
            return player.deck.field.Contains(card);
        }

    }
}
