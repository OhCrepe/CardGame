﻿using Server.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class GameState
    {

        public PlayerHandler player1, player2;

        public bool targetting = false, attacking = false, deciding = false;
        public Card targettingCard = null, decidingCard = null;

        public enum Phase { START, DEBT, GOLD, START_EFFECTS, DRAW, MAIN, END_EFFECTS, END };
        public static Phase currentPhase;

        public PlayerHandler currentPlayer;

        private const int startingGold = 25;
        private const int goldPerTurn = 5;

        public GameState(PlayerHandler p1)
        {
            player1 = p1;
            p1.game = this;
        }

        /*  
        *   Initialize the Game state
        */
        public void InitializeGame()
        {

            if (player1 != null && player2 != null)
            {
                currentPlayer = player1;
                player1.otherPlayer = player2;
                player2.otherPlayer = player1;
                player1.deck.ShuffleDeck();
                for (int i = 0; i < 4; i++)
                {
                    player1.deck.DrawCard();
                }
                player1.SetGold(startingGold);
                player2.deck.ShuffleDeck();
                for (int i = 0; i < 4; i++)
                {
                    player2.deck.DrawCard();
                }
                player2.SetGold(startingGold);
                MainPhase();
            }

        }


        /*
        *	Logic that handles the start of a player's turn
        */
        public void StartPhase()
        {
            currentPhase = Phase.START;
            DebtPhase();
        }

        /*
        *	Logic that handles checking for a player's debt
        * Give them a debt counter if no gold left, or they lose the game if they have
        * no gold AND a debt counter
        */
        public void DebtPhase()
        {
            currentPhase = Phase.DEBT;
            GoldPhase();
        }

        /*
        *	Logic that handles the player's gold gained for the turn.
        * DEFUALT: Give 5 gold.
        */
        public void GoldPhase()
        {
            currentPhase = Phase.GOLD;
            player1.SetGold(player1.gold += goldPerTurn);
            StartEffectsPhase();
        }

        /*
        * Resolve all effects that trigger at the beginning of a player's turn
        */
        public void StartEffectsPhase()
        {
            currentPhase = Phase.START_EFFECTS;
            DrawPhase();
        }

        /*
        *	Turn player draws a card
        */
        public void DrawPhase()
        {
            currentPhase = Phase.DRAW;
            player1.deck.DrawCard();
            MainPhase();
        }

        /*
        *	In this phase the turn player gets to play their cards, hire units
        * and enter combat
        */
        public void MainPhase()
        {
            currentPhase = Phase.MAIN;
        }

        /*
        *	Resolve all effects that trigger at the end of a player's turn
        */
        public void EndEffectsPhase()
        {

            foreach(Card card in player1.deck.field)
            {
                card.ability.EndOfTurnAbility();
            }
            player1.deck.lord.ability.EndOfTurnAbility();
            EndPhase();

        }

        /*
        *	End the current player's turn, give control to their opponent
        */
        public void EndPhase()
        {
            currentPhase = Phase.END;
            player1.deck.PayWages();
            StartPhase();
        }

        /*
        *   Validate that the turn player can legally end their turn
        */
        public bool ValidateEndMainPhase()
        {

            if (currentPhase != Phase.MAIN) return false;

            return true;

        }

        /*
        *   Validate that a summon is legal
        */ 
        public bool ValidateSummon(string id)
        {

            Card card = player1.deck.GetCardFromHand(id);
            if (card == null)
            {
                return false;
            }
            if(card.cardType == Card.Type.UTILITY && !card.ability.ValidActivation())
            {
                return false;
            }
            if(currentPlayer.gold < card.cost)
            {
                return false;
            }
            return true;

        }

        /*
        *   Summon a unit to the field
        */
        public void SummonUnit(string id)
        {
            Card card = player1.deck.GetCardFromHand(id);
            player1.deck.hand.Remove(card);
            player1.deck.field.Add(card);
            player1.SetGold(player1.gold -= card.cost);
            player1.SendMessage("SUMMON#" + id);
            card.ability.OnHire();
        }

        /*
         * Looks for the card with the given id
         */ 
        public Card FindCard(string id)
        {
            if (player1.deck.lord.id == id) return player1.deck.lord;
            foreach(Card card in player1.deck.deck)
            {
                if (card.id == id) return card;
            }
            foreach (Card card in player1.deck.hand)
            {
                if (card.id == id) return card;
            }
            foreach (Card card in player1.deck.field)
            {
                if (card.id == id) return card;
            }
            foreach (Card card in player1.deck.discard)
            {
                if (card.id == id) return card;
            }
            return null;
        }

        /*
         * Tell the client we are now targetting cards
         */ 
        public void TargetCard(CardAbility ability)
        {
            targetting = true;
            targettingCard = ability.card;
            player1.SendMessage("TARGET#" + targettingCard.id);
        }

        /*
         * Check that an attack is valid
         */
        public bool ValidAttack(Card attacking, Card attacked)
        {
            if (attacking == attacked) return false;
            return player1.deck.field.Contains(attacking) && player1.deck.field.Contains(attacked);
        }

        /*
         * Resolve combat between 2 units
         */ 
        public void Attack(Card attacking, Card attacked)
        {
            attacking.DealDamage(attacked.currentStrength, true);
            attacked.DealDamage(attacking.currentStrength, true);
        }

    }

}
