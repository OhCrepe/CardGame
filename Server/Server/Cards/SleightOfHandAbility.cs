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

        public override void OnHire()
        {
            game.TargetCard(this);
        }

        public override bool ValidActivation()
        {
            return (game.player1Deck.hand.Count > 1 && game.player1Deck.deck.Count > 1 && game.player1Deck.hand.Contains(card));
        }

        public override bool ValidateTarget(Card card)
        {
            return game.player1Deck.hand.Contains(card);
        }

        public override void OnTargetSelect(Card card)
        {
            game.player1Deck.ShuffleIntoDeck(card);
            game.player1Deck.DrawCard();
            game.player1Deck.DrawCard();
            base.OnTargetSelect(card);
            CheckUtility();
        }
    }
}
