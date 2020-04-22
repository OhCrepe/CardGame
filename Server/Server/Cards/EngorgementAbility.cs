using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    public class EngorgementAbility : CardAbility
    {

        public const int strengthGained = 2;

        public EngorgementAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
         * Target a card on activation
         */ 
        public override void OnHire()
        {
            game.TargetCard(this);
        }

        /*
        * Ensure this card is in hand and that there is a target for it's effect, and that there are cards to draw
        */
        public override bool ValidActivation()
        {
            if (!player.deck.hand.Contains(card)) return false;
            return (player.deck.field.Count > 0 || player.otherPlayer.deck.field.Count > 0);
        }

        /*
         * Validate the target is in hand
         */ 
        public override bool ValidateTarget(Card card)
        {
            if (card.cardType != Card.Type.MINION) return false;
            return player.deck.field.Contains(card) || player.otherPlayer.deck.field.Contains(card);
        }

        /*
         * On selecting the target, shuffle it into the deck and draw 2 cards
         */ 
        public override void OnTargetSelect(Card card)
        {
            card.IncreaseStrength(strengthGained);
            base.OnTargetSelect(card);
            CheckUtility();
        }
    }
}
