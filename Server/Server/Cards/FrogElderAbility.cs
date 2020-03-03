using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class FrogElderAbility : CardAbility
    {

        public const int abilityCost = 3;
        public const string searchString = "Frog";

        public FrogElderAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
        {

        }

        /*
         * Tell the client we're going to search for a card on trigger
         */ 
        public override void OnFieldTrigger()
        {
            if (player.deck.CardInDeckWithString(searchString))
            {
                game.TargetCard(this);
                SearchCard(searchString);
            }
        }

        /*
         * Validate that the chosen card is in deck and a valid search target
         */ 
        public override bool ValidateTarget(Card card)
        {
            return player.deck.deck.Contains(card) && card.name.Contains(searchString);
        }

        /*
         * Add to chosen card to hand
         */ 
        public override void OnTargetSelect(Card card)
        {
            player.SetGold(player.gold - abilityCost);
            player.deck.MoveCardToHand(card);
            player.SendMessage("CLOSE_SEARCH");
            Bounce();
        }

    }
}
