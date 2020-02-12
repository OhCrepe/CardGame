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
    class Server
    {

        private const int port = 8888;

        static void Main(string[] args)
        {

            RunServer();

        }

        private static void RunServer()
        {

            TcpListener listener = null;
            try
            {
                listener = new TcpListener(IPAddress.Loopback, port);
                listener.Start();
                Console.WriteLine("Waiting for incoming connections...");
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    PlayerHandler handler = new PlayerHandler(client);
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
