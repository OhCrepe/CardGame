using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class CannibalAbility : CardAbility
    {

        public CannibalAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {
            isOncePerTurn = true;
        }

        /*
         * Target a card on ability usage, assuming we haven't done so this turn already
         */ 
        public override void OnFieldTrigger()
        {
            if(!oncePerTurnUsed) game.TargetCard(this);
        }

        /*
         * Make sure there's another card to kill
         */
        public override bool ValidActivation()
        {
            return player.deck.field.Count > 1 && player.deck.field.Contains(card);
        }

        /*
         *  Validate this target is on the field
         */
        public override bool ValidateTarget(Card card)
        {
            return player.deck.field.Contains(card) && card.id != this.card.id && card.cardType == Card.Type.MINION;
        }

        /*
         * Pay a little gold and heal the card on target select
         */ 
        public override void OnTargetSelect(Card card)
        {
            if (oncePerTurnUsed) return;
            card.ability.Kill(false);
            this.card.Heal(card.health);
            oncePerTurnUsed = true;
            player.SendMessage("OPT_USED#" + this.card.id);
        }

    }
}
