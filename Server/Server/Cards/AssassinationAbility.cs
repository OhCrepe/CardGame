using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class AssassinationAbility : CardAbility
    {

        public AssassinationAbility(PlayerHandler player, Card card, GameState game) : base(player, card, game)
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
        *   Can only be activated if there's a unit on the field
        */ 
        public override bool ValidActivation()
        {
            if ((player.deck.field.Count > 1 || player.otherPlayer.deck.field.Count > 1) && player.deck.hand.Contains(card))
            {
                foreach(Card card in player.deck.field)
                {
                    if(card.currentHealth >= card.health)
                    {
                        return true;
                    }
                }
                foreach (Card card in player.otherPlayer.deck.field)
                {
                    if (card.currentHealth >= card.health)
                    {
                        return true;
                    }
                }
                return false;
            }
            else return false;
        }

        /*
        *   Our target must be in the hand
        */
        public override bool ValidateTarget(Card card)
        {
            Console.WriteLine("Is targetted");
            if (card.cardType != Card.Type.MINION) return false;
            Console.WriteLine("Is a minion");
            if (card.currentHealth < card.health) return false;
            Console.WriteLine("Is healthy");
            return (player.deck.field.Contains(card) || player.otherPlayer.deck.field.Contains(card));
        }

        /*
        * On selecting the target, discard it, and give ourselves gold equal to its cost
        */
        public override void OnTargetSelect(Card card)
        {
            card.DealDamage(card.currentHealth, false);
            base.OnTargetSelect(card);
            CheckUtility();
        }

    }
}
