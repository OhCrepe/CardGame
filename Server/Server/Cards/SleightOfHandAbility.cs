using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class SleightOfHandAbility : CardAbility
    {

        public SleightOfHandAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
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
        * (we do shuffle in first so we just need 1 card in deck)
        */
        public override bool ValidActivation()
        {
            return (player.deck.hand.Count > 1 && player.deck.deck.Count > 0 && player.deck.hand.Contains(card));
        }

        /*
         * Validate the target is in hand
         */ 
        public override bool ValidateTarget(Card card)
        {
            return player.deck.hand.Contains(card);
        }

        /*
         * On selecting the target, shuffle it into the deck and draw 2 cards
         */ 
        public override void OnTargetSelect(Card card)
        {
            player.deck.ShuffleIntoDeck(card);
            player.deck.DrawCard();
            player.deck.DrawCard();
            base.OnTargetSelect(card);
            CheckUtility();
        }
    }
}
