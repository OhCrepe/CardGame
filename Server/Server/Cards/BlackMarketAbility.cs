using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class BlackMarketAbility : CardAbility
    {

        public BlackMarketAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        public override void OnHire()
        {
            game.TargetCard(this);
        }

        public override bool ValidActivation()
        {
            return (game.player1Deck.hand.Count > 0);
        }

        public override bool ValidateTarget(Card card)
        {
            return game.player1Deck.hand.Contains(card);
        }

        public override void OnTargetSelect(Card card)
        {
            game.player1Deck.hand.Remove(card);
            game.player1Deck.discard.Add(card);
            player.SendMessage("KILL#" + card.id);
            player.SetGold(player.gold += card.cost);
            base.OnTargetSelect(card);
            CheckUtility();
        }

    }
}
