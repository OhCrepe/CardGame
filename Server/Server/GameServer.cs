using Server.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class GameServer
    {

        private const string regularIP = "192.168.1.16";
        private const int port = 8888;

        static void Main(string[] args)
        {

            Config.LoadSettings();
            CardReader.StartReader();
            RunServer();

        }

        /*
         * Listen for clients connecting
         */ 
        private static void RunServer()
        {
            GameState game = null;
            TcpListener listener = null;
            try
            {

                string ip = regularIP;
                if (Config.configSettings.ContainsKey("test_local_server"))
                {
                    if (Config.configSettings["test_local_server"] == "true")
                    {
                        listener = new TcpListener(IPAddress.Loopback, port);
                        Console.WriteLine("Starting local server...");
                    }
                    else
                    {
                        listener = new TcpListener(IPAddress.Parse(ip), port);
                    }
                }
                else
                {
                    listener = new TcpListener(IPAddress.Parse(ip), port);
                }
                listener.Start();

                Console.WriteLine("Waiting for incoming connections...");
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    PlayerHandler handler = new PlayerHandler(client);
                    if(game == null || !game.player1.IsConnected())
                    {
                        game = new GameState(handler);
                        Console.WriteLine("Player 1 connected to game " + handler.game.id);
                    }
                    else
                    {
                        game.player2 = handler;
                        handler.game = game;
                        Console.WriteLine("Player 2 connected to game " + handler.game.id);
                        game = null;
                    }              
                    Thread thread = new Thread(new ThreadStart(handler.Run));
                    thread.Start();
                    Console.WriteLine("Connection established");
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("There was a problem creating a TcpListener");
            }

        }

    }
}
