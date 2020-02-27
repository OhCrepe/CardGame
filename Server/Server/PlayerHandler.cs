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

        private TcpClient client;
        private StreamReader input;
        private StreamWriter writer;
        private GameState game;

        public PlayerHandler(TcpClient client)
        {

            this.client = client;
            try
            {
                NetworkStream stream = client.GetStream();
                input = new StreamReader(stream);
                writer = new StreamWriter(stream);
                writer.AutoFlush = true;
                game = new GameState(this);
            }
            catch (SocketException)
            {
                Console.WriteLine("There was a problem creating the PlayerHandler");
            }

        }

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

                            case "ATTACK":
                                Card attackingCard = game.FindCard(args[1]);
                                Card attackTarget = game.FindCard(args[2]);
                                if (attackingCard == null || attackTarget == null) break;
                                if (game.ValidAttack(attackingCard, attackTarget))
                                {
                                    game.Attack(attackingCard, attackTarget);
                                }
                                break;

                            case "DECKLIST":
                                ReadDeckList(args);
                                game.InitializeGame();
                                break;

                            case "ENDTURN":
                                if (game.ValidateEndMainPhase())
                                {
                                    game.EndEffectsPhase();
                                }
                                break;

                            case "SUMMON":
                                if (game.ValidateSummon(args[1]))
                                {
                                    game.SummonUnit(args[1]);
                                }
                                break;

                            case "TARGET":
                                if (game.targetting == false) break;
                                Card target = game.FindCard(args[1]);
                                if (game.targettingCard.ability.ValidateTarget(target))
                                {
                                    SendMessage("VALID_TARGET");
                                    game.targettingCard.ability.OnTargetSelect(target);
                                }
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

        private void ReadDeckList(string[] args)
        {
            Card lord = new Card(CardReader.cardStats[args[1]], this, game);
            Card[] deck = new Card[25];
            for(int i = 2; i < args.Length; i++)
            {
                deck[i - 2] = new Card(CardReader.cardStats[args[i]], this, game);
            }
            game.player1Deck = new ServerDeckList(this, lord, deck);
        }

        public void SetGold(int gold)
        {
            this.gold = gold;
            SendMessage("GOLD#" + gold);
        }

        public void SendMessage(string message)
        {
            Console.WriteLine("Sending Message: " + message);
            writer.WriteLine(message);
        }

    }
}
