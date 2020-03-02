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

        public override void OnFieldTrigger()
        {
            if(!oncePerTurnUsed) game.TargetCard(this);
        }

        public override bool ValidateTarget(Card card)
        {
            return game.player1Deck.field.Contains(card) && card.cardType == Card.Type.MINION;
        }

        public override void OnTargetSelect(Card card)
        {
            player.SetGold(player.gold - abilityCost);
            card.Heal(healAmount);
            oncePerTurnUsed = true;
        }

    }
}
