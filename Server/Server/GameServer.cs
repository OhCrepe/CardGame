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

        private const string ip = "192.168.0.16";
        private const int port = 8888;

        static void Main(string[] args)
        {

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
                listener = new TcpListener(IPAddress.Parse(ip), port);
                listener.Start();
                Console.WriteLine("Waiting for incoming connections...");
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    PlayerHandler handler = new PlayerHandler(client);
                    if(game == null)
                    {
                        game = new GameState(handler);
                    }else
                    {
                        game.player2 = handler;
                        handler.game = game;
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
