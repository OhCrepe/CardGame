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
        */
        public override bool ValidActivation()
        {
            return (game.player1.deck.hand.Count > 1 && game.player1.deck.deck.Count > 1 && game.player1.deck.hand.Contains(card));
        }

        /*
         * Validate the target is in hand
         */ 
        public override bool ValidateTarget(Card card)
        {
            return game.player1.deck.hand.Contains(card);
        }

        /*
         * On selecting the target, shuffle it into the deck and draw 2 cards
         */ 
        public override void OnTargetSelect(Card card)
        {
            game.player1.deck.ShuffleIntoDeck(card);
            game.player1.deck.DrawCard();
            game.player1.deck.DrawCard();
            base.OnTargetSelect(card);
            CheckUtility();
        }
    }
}
