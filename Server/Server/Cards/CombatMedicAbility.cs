using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class CombatMedicAbility : CardAbility
    {

        public int abilityCost = 1, healAmount = 2;

        public CombatMedicAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
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
         *  Validate this target is on the field
         */ 
        public override bool ValidateTarget(Card card)
        {
            return player.deck.field.Contains(card) && card.cardType == Card.Type.MINION;
        }

        /*
         * Pay a little gold and heal the card on target select
         */ 
        public override void OnTargetSelect(Card card)
        {
            if (oncePerTurnUsed) return;
            player.SetGold(player.gold - abilityCost);
            card.Heal(healAmount);
            oncePerTurnUsed = true;
            player.SendMessage("OPT_USED#" + this.card.id);
        }

    }
}
