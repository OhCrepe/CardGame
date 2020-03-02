﻿using System;
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

        /*
        *   Upon being played, target a card
        */ 
        public override void OnHire()
        {
            game.TargetCard(this);
        }

        /*
        *   Can only be activated if this card is in our hand and we have at least 1 other card in hand
        */ 
        public override bool ValidActivation()
        {
            return (game.player1Deck.hand.Count > 1 && game.player1Deck.hand.Contains(card));
        }

        /*
        *   Our target must be in the hand
        */
        public override bool ValidateTarget(Card card)
        {
            return game.player1Deck.hand.Contains(card);
        }

        /*
        * On selecting the target, discard it, and give ourselves gold equal to its cost
        */
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
