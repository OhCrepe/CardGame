using Server.Cards;
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
            if (currentPlayer == player1)
            {
                currentPlayer = player2;
            }
            else currentPlayer = player1;
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
            currentPlayer.SetGold(currentPlayer.gold += goldPerTurn);
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
            currentPlayer.deck.DrawCard();
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

            foreach(Card card in currentPlayer.deck.field)
            {
                card.ability.EndOfTurnAbility();
            }
            currentPlayer.deck.lord.ability.EndOfTurnAbility();
            EndPhase();

        }

        /*
        *	End the current player's turn, give control to their opponent
        */
        public void EndPhase()
        {
            currentPhase = Phase.END;
            currentPlayer.deck.PayWages();
            StartPhase();
        }

        /*
        *   Validate that the turn player can legally end their turn
        */
        public bool ValidateEndMainPhase(PlayerHandler player)
        {

            if (currentPlayer != player) return false;
            if (currentPhase != Phase.MAIN) return false;

            return true;

        }

        /*
        *   Validate that a summon is legal
        */ 
        public bool ValidateSummon(string id, PlayerHandler player)
        {

            if (currentPlayer != player) return false;

            Card card = currentPlayer.deck.GetCardFromHand(id);
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
            Card card = currentPlayer.deck.GetCardFromHand(id);
            currentPlayer.deck.hand.Remove(card);
            currentPlayer.deck.field.Add(card);
            currentPlayer.SetGold(player1.gold -= card.cost);
            currentPlayer.SendMessage("SUMMON#" + id);
            currentPlayer.otherPlayer.SendMessage("RFH");

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
            if (player2.deck.lord.id == id) return player2.deck.lord;
            foreach (Card card in player2.deck.deck)
            {
                if (card.id == id) return card;
            }
            foreach (Card card in player2.deck.hand)
            {
                if (card.id == id) return card;
            }
            foreach (Card card in player2.deck.field)
            {
                if (card.id == id) return card;
            }
            foreach (Card card in player2.deck.discard)
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
            currentPlayer.SendMessage("TARGET#" + targettingCard.id);
        }

        /*
         * Check that an attack is valid
         */
        public bool ValidAttack(Card attacking, Card attacked)
        {
            if (attacking == attacked) return false;
            return currentPlayer.deck.field.Contains(attacking) && currentPlayer.otherPlayer.deck.field.Contains(attacked);
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
