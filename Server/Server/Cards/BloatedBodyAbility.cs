using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class BloatedBodyAbility : CardAbility
    {

        public const int deathDamage = 3;

        public BloatedBodyAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        public override void OnKillAbility(bool combat)
        {

            if (game.player1Deck.field.Contains(card))
            {
                return;
            }
            foreach(Card card in game.player1Deck.field.ToList())
            {
                card.DealDamage(deathDamage, false);
            }

        }

    }
}
