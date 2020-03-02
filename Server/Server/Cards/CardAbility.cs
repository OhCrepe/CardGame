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

        public virtual void Attack()
        {

        }


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
            /*
            PlayerField playerField = this.player.GetComponent<PlayerField>();
            if (playerField.gold > GetComponent<CardData>().cost)
            {
                return ActivationRequirementsMet();
            }
            return false;
            */
            return true;
        }

        protected virtual bool ActivationRequirementsMet()
        {
            return true;
        }

        public virtual void OnKillByCombatAbility()
        {
            //DO NOTHING
        }
        public virtual void OnKillByEffectAbility()
        {
            //DO NOTHING
        }

        // What to do when a target is selected for this cards ability
        public virtual void OnTargetSelect(Card card)
        {
            game.targetting = false;
            game.targettingCard = null;
        }

        //Validate that the target for this ability is valid
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
        }

        public void CheckUtility()
        {
            if (card.cardType == Card.Type.UTILITY)
            {
                Kill(false);
            }
        }


        protected virtual bool ValidAttackTarget(Card target)
        {

            /*
            if (target.transform.parent.GetComponent<Dropzone>().zoneType == Dropzone.Zone.FIELD)
            {
                return true;
            }
            return false;
            */
            return true;

        }

        public void OnAttackTargetSelect(Card target)
        {

            //GetComponent<CardData>().DealDamageTo(target, true);
            //target.GetComponent<CardData>().DealDamageTo(gameObject, true);

        }

        public void Kill(bool combat)
        {

            if (game.player1Deck.hand.Contains(card))
            {
                game.player1Deck.hand.Remove(card);
            }
            if (game.player1Deck.deck.Contains(card))
            {
                game.player1Deck.deck.Remove(card);
            }
            if (game.player1Deck.field.Contains(card))
            {
                game.player1Deck.field.Remove(card);
            }
            if (game.player1Deck.discard.Contains(card))
            {
                return;
            }
            game.player1Deck.discard.Add(card);
            player.SendMessage("KILL#" + card.id);
            OnKillAbility(combat);
            //player.GetComponent<PlayerField>().discard.GetComponent<DiscardPile>().Discard(this.gameObject);
            //OnKillAbility(combat);

        }

        public void Bounce()
        {
            /*
            Transform hand = player.GetComponent<PlayerField>().hand.transform;
            GetComponent<Draggable>().parentToReturnTo = hand;
            this.gameObject.transform.SetParent(hand);
            gameObject.SetActive(true);
            GetComponent<CardData>().Restore();
            */
        }

        public void ResetOncePerTurn()
        {
            oncePerTurnUsed = false;
        }

    }
}
