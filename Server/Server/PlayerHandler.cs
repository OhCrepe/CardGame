using Server.Cards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class PlayerHandler : IDisposable
    {

        public int gold;
        public ServerDeckList deck;

        private TcpClient client;
        private StreamReader input;
        private StreamWriter writer;
        public GameState game;
        public PlayerHandler otherPlayer;

        public PlayerHandler(TcpClient client)
        {

            this.client = client;
            try
            {
                NetworkStream stream = client.GetStream();
                input = new StreamReader(stream);
                writer = new StreamWriter(stream);
                writer.AutoFlush = true;
            }
            catch (SocketException)
            {
                Console.WriteLine("There was a problem creating the PlayerHandler");
            }

        }

        /*
         * Close the connection
         */ 
        public void Dispose()
        {
            input.Close();
            writer.Close();
            client.Close();
        }

        public void Run()
        {
            try
            {
                bool connected = true;
                while (connected)
                {
                    String line = input.ReadLine();
                    if (line != null)
                    {
                        Console.WriteLine("Recieved: " + line);
                        String[] args = line.Trim().Split('#');
                        switch (args[0].ToUpper())
                        {

                            // Player attempts to attack
                            case "ATTACK":
                                if (game.currentPlayer != this) break;
                                Card attackingCard = game.FindCard(args[1]);
                                Card attackTarget = game.FindCard(args[2]);
                                if (attackingCard == null || attackTarget == null) break;
                                if (game.ValidAttack(attackingCard, attackTarget))
                                {
                                    game.Attack(attackingCard, attackTarget);
                                }
                                break;

                            case "ATTACK_DIRECT":
                                if (game.currentPlayer != this) break;
                                Card directCard = game.FindCard(args[1]);
                                if (directCard == null) break;
                                if (game.ValidDirectAttack(directCard))
                                {
                                    game.DirectAttack(directCard);
                                }
                                break;

                            // Player sends decklist
                            case "DECKLIST":
                                ReadDeckList(args);
                                game.InitializeGame();
                                break;

                            // Player attempts to end their turn
                            case "ENDTURN":
                                if (game.currentPlayer != this) break;
                                if (game.ValidateEndMainPhase(this))
                                {
                                    game.EndEffectsPhase();
                                }
                                break;

                            // Player attempts to activate a field effect
                            case "FIELD_EFFECT":
                                if (game.currentPlayer != this) break;
                                Card card = game.FindCard(args[1]);
                                if (card.ability.ValidActivation())
                                {
                                    card.ability.OnFieldTrigger();
                                }
                                break;

                            // Player attempts to summon
                            case "SUMMON":
                                if (game.ValidateSummon(args[1], this))
                                {
                                    game.SummonUnit(args[1]);
                                }
                                else
                                {
                                    SendMessage("INVALID#" + args[1]);
                                }
                                break;

                            // Player attempts to target
                            case "TARGET":
                                if (game.currentPlayer != this) break;
                                if (game.targetting == false) break;
                                Card target = game.FindCard(args[1]);
                                if(target == null)
                                {
                                    Console.WriteLine("Null");
                                }
                                if (game.targettingCard.ability.ValidateTarget(target))
                                {
                                    SendMessage("VALID_TARGET");
                                    game.targettingCard.ability.OnTargetSelect(target);
                                }
                                Console.WriteLine("Invalid");
                                break;

                            default:
                                break;

                        }
                    }
                }
            }
            catch (Exception e)
            {
                try
                {
                    client.Close();
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex);
                }
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Player disconnected");
            }
        }

        /*
         * Read the decklist given by the client
         */ 
        private void ReadDeckList(string[] args)
        {
            Card lord = new Card(CardReader.cardStats[args[1]], this, game);
            Card[] deck = new Card[25];
            for(int i = 2; i < args.Length; i++)
            {
                deck[i - 2] = new Card(CardReader.cardStats[args[i]], this, game);
            }
            this.deck = new ServerDeckList(this, lord, deck);
        }

        /*
         * Change the value of gold for this player
         */ 
        public void SetGold(int gold)
        {

            if(gold <= 0)
            {
                gold = 0;
                SendMessage("LOSE");
                otherPlayer.SendMessage("WIN");
            }
            else {
                this.gold = gold;
            }

            SendMessage("GOLD#" + gold);
            otherPlayer.SendMessage("GOLD_OPP#" + gold);

            if (gold <= 0) game.gameOver = true;

        }

        /*
         * Send a message to this player
         */ 
        public void SendMessage(string message)
        {
            if (game.gameOver) return;
            Console.WriteLine("Sending Message: " + message);
            writer.WriteLine(message);
        }

    }
}
