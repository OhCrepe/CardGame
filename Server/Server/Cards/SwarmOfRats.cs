﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class SwarmOfRatsAbility : CardAbility
    {

        private const int damageDealt = 2;

        public SwarmOfRatsAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
        * The ability of this card that triggers on summon
        * In this case, we search for another copy of this card (if there is one)
        */
        public override void OnHire()
        {

            foreach (Card cardInDeck in player.deck.deck.ToList())
            {
                if(cardInDeck.name == this.card.name)
                {
                    player.deck.MoveCardToHand(cardInDeck);
                    return;
                }
            }
            return;

        }

    }
}