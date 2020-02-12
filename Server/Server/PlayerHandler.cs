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

        private TcpClient client;
        private StreamReader input;
        private StreamWriter writer;

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
                        String[] args = line.Trim().Split('#');
                        switch (args[0].ToUpper())
                        {
                            case "DECKLIST":
                                ReadDeckList(args);
                                GameState.serverDeckList.PrintDeckList();
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
            string lord = args[1];
            string[] deck = new string[25];
            for(int i = 2; i < args.Length; i++)
            {
                deck[i - 2] = args[i];
            }
            GameState.serverDeckList = new ServerDeckList(lord, deck);
        }

    }
}
