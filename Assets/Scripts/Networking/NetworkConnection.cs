using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetworkConnection : MonoBehaviour, IDisposable
{

    private StreamReader reader;
    private StreamWriter writer;
    private TcpClient tcp;

    private DeckInteraction deck;
    private PlayerField player;

    private List<string> commands;

    private const int port = 8888;

    // Initialize the network connection
    void Awake()
    {
        deck = GameObject.Find("PlayerField").GetComponent<DeckInteraction>();
        player = GameObject.Find("PlayerField").GetComponent<PlayerField>();
        commands = new List<string>();
        InitializeRemoteIO();
        InitializeListenThread();
    }

    /*
    *   Process commands given to use by the server
    */
    void Update(){

        if(commands.Count > 0){
            string line = commands[0];
            string[] args = line.Trim().Split('#');
            Debug.Log("Receiving: " + line);
            GameObject card;
            switch(args[0].ToUpper()){

                // Add a card to hand
                case "ATH":
                    deck.AddToHand(args[1]);
                    break;

                // Bounce a card to hand
                case "BOUNCE":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    card.GetComponent<CardAbility>().Bounce();
                    break;

                // Close the search window
                case "CLOSE_SEARCH":
                    deck.CloseSearchWindow();
                    break;

                // Deal damage to a card
                case "DAMAGE":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    card.GetComponent<CardData>().DealDamage(int.Parse(args[2]));
                    break;

                // Change the value of a player's gold
                case "GOLD":
                    player.SetGold(int.Parse(args[1]));
                    break;

                // Heal a unit
                case "HEAL":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    card.GetComponent<CardData>().Heal(int.Parse(args[2]));
                    break;

                // Assign an id to a card
                case "ID":
                    deck.AssignIdToCardWithName(args[1], args[2]);
                    break;

                // Invalid activation, put that card back on the field
                case "INVALID":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    card.GetComponent<CardAbility>().Bounce();
                    break;

                // Kill a card
                case "KILL":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    GameObject.Find("Discard").GetComponent<DiscardPile>().Discard(GameObject.Find(args[1]));
                    break;

                // Put a card into the deck
                case "PUTINDECK":
                    deck.ShuffleIntoDeck(GameObject.Find(args[1]));
                    break;

                // Restore a cards stats
                case "RESTORE":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    card.GetComponent<CardData>().Restore();
                    break;

                // Search for a card
                case "SEARCH":
                    deck.SearchCardFromDeckByName(args[1]);
                    break;

                // Summon a card
                case "SUMMON":
                    player.SummonUnitWithId(args[1]);
                    break;

                // Target a card
                case "TARGET":
                    GameState.targetting = true;
                    GameState.targettingCard = GameObject.Find(args[1]);
                    break;

                // The given target is valid
                case "VALID_TARGET":
                    GameState.targetting = false;
                    GameState.targettingCard = null;
                    break;

                default:
                    break;

            }

            commands.RemoveAt(0);

        }
    }

    /*
    *   Initialize the stream reader/writer and tcp client
    */
    private void InitializeRemoteIO(){

        tcp = new TcpClient("localhost", port);
        NetworkStream stream = tcp.GetStream();
        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);
        writer.AutoFlush = true;

    }

    /*
    *   Start the thread that listens to the server
    */
    private void InitializeListenThread(){
        Thread thread = new Thread(new ThreadStart(ListenThread));
        thread.Start();
        Console.WriteLine("Connection established");
    }

    /*
    *   Listen for commands given by the server and add them to the queue
    */
    private void ListenThread(){

        bool connected = true;
        while(connected){

            string line = reader.ReadLine();
            if(line != null){

                commands.Add(line);

            }

        }

    }

    /*
    *   Send a message to the server
    */
    public void SendMessage(string message){
        Debug.Log(message);
        writer.WriteLine(message);
    }

    /*
     * Close the connection
     */
    public void Dispose()
    {
        reader.Close();
        writer.Close();
        tcp.Close();
    }

}
