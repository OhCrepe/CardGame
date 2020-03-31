using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class TheContractAbility : CardAbility
    {

        private const string searchName = "Assassin";

        public TheContractAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
         * At the end of the turn, heal all your units for 1 hp
         */
        public override void EndOfTurnAbility()
        {
            if (game.currentPlayer == player) return;
            foreach (Card cardInDeck in player.deck.deck.ToList())
            {
                if (cardInDeck.name == searchName)
                {
                    player.deck.MoveCardToHand(cardInDeck);
                    return;
                }
            }
            return;

        }


    }
}
