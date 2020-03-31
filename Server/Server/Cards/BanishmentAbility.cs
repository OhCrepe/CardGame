using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class BanishmentAbility : CardAbility
    {

        public BanishmentAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
        *   Upon being played, target a card
        */ 
        public override void OnHire()
        {
            game.TargetCard(this);
        }

        /*
        *   Can only be activated if there's a unit on the field
        */ 
        public override bool ValidActivation()
        {
            if ((player.deck.field.Count > 0 || player.otherPlayer.deck.field.Count > 0) && player.deck.hand.Contains(card))
            {
                return true;
            }
            else return false;
        }

        /*
        *   Our target must be in the hand
        */
        public override bool ValidateTarget(Card card)
        {
            if (card.cardType != Card.Type.MINION) return false;
            return (player.deck.field.Contains(card) || player.otherPlayer.deck.field.Contains(card));
        }

        /*
        * On selecting the target, discard it, and give ourselves gold equal to its cost
        */
        public override void OnTargetSelect(Card card)
        {
            card.Restore();
            card.player.deck.ShuffleIntoDeck(card);
            base.OnTargetSelect(card);
            CheckUtility();
        }

    }
}
