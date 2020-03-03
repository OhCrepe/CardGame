using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Cards
{
    class CardAbility
    {

        public PlayerHandler player; // The player this card belongs to
        public Card card;
        public GameState game;
        public int fieldTriggerCost; //The cost to trigger the field effect of this card
        public bool hasTriggerAbility, //Whether this card has an ability that can be triggered on the field
                                hasStartOfTurnAbility,
                                hasEndOfTurnAbility;
        public bool isOncePerTurn;

        public bool oncePerTurnUsed;
        public Card target = null;

        public CardAbility(PlayerHandler player, Card card, GameState game)
        {
            this.player = player;
            this.card = card;
            this.game = game;
            oncePerTurnUsed = false;
        }

        /*
         * Attack a card
         */ 
        public virtual void Attack()
        {

        }

        /*
         * Ability that triggers on dying
         */ 
        public virtual void OnKillAbility(bool combat)
        {
            if (combat)
            {
                OnKillByCombatAbility();
            }
            else
            {
                OnKillByEffectAbility();
            }
        }

        /*
        *	Check whether the activation of the card is legal
        */
        public virtual bool ValidActivation()
        {
            return true;
        }

        /*
         *  Check the activation requirements of the card
         */ 
        protected virtual bool ActivationRequirementsMet()
        {
            return true;
        }

        /*
         *  Triggers when killed by combat
         */ 
        public virtual void OnKillByCombatAbility()
        {
            //DO NOTHING
        }

        /*
         *  Triggers when killed by card effect
         */
        public virtual void OnKillByEffectAbility()
        {
            //DO NOTHING
        }

        /*
         *  What to do when a target is selected
         */
        public virtual void OnTargetSelect(Card card)
        {
            game.targetting = false;
            game.targettingCard = null;
        }

        /*
         *  Validate that the target is a legal one
         */
        public virtual bool ValidateTarget(Card card)
        {
            return true;
        }

        // Ability that triggers when hired
        public virtual void OnHire()
        {
            CheckUtility();
        }

        // Ability that triggers on the field when clicked
        public virtual void OnFieldTrigger()
        {
            //DO NOTHING
        }

        // Ability that triggers on the field when clicked
        public virtual void StartOfTurnAbility()
        {
            //DO NOTHING
        }

        // Ability that triggers on the field when clicked
        public virtual void EndOfTurnAbility()
        {
            //DO NOTHING
            oncePerTurnUsed = false;
        }


        /*
         *  Search for a card from deck
         */
        public virtual void SearchCard(string searchString)
        {
            player.SendMessage("SEARCH#" + searchString);
        }


        /*
         *  If its a utility, destroy it
         */
        public void CheckUtility()
        {
            if (card.cardType == Card.Type.UTILITY)
            {
                Kill(false);
            }
        }


        /*
         *  Check it's a valid attack target
         */
        protected virtual bool ValidAttackTarget(Card target)
        {

            return true;

        }

        /*
         * Kill this card
         */ 
        public virtual void Kill(bool combat)
        {

            if (player.deck.hand.Contains(card))
            {
                player.deck.hand.Remove(card);
            }
            if (player.deck.deck.Contains(card))
            {
                player.deck.deck.Remove(card);
            }
            if (player.deck.field.Contains(card))
            {
                player.deck.field.Remove(card);
            }
            if (player.deck.discard.Contains(card))
            {
                return;
            }
            player.deck.discard.Add(card);
            player.SendMessage("KILL#" + card.id);
            player.otherPlayer.SendMessage("KILL_OPP#" + card.id);
            OnKillAbility(combat);

        }

        /*
         * Bounce this card - return it to hand
         */ 
        public void Bounce()
        {
            if (player.deck.discard.Contains(card))
            {
                player.deck.discard.Remove(card);
            }
            if (player.deck.deck.Contains(card))
            {
                player.deck.deck.Remove(card);
            }
            if (player.deck.field.Contains(card))
            {
                player.deck.field.Remove(card);
            }
            if (player.deck.hand.Contains(card))
            {
                return;
            }
            player.deck.hand.Add(card);
            player.SendMessage("BOUNCE#" + card.id);
            player.otherPlayer.SendMessage("BOUNCE_OPP#" + card.id);
            card.Restore();

        }

        /*
         * Reset this cards once per turnness
         */ 
        public void ResetOncePerTurn()
        {
            oncePerTurnUsed = false;
        }

    }
}
