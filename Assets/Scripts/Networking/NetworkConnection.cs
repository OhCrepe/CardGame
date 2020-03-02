using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetworkConnection : MonoBehaviour
{

    private StreamReader reader;
    private StreamWriter writer;
    private TcpClient tcp;

    private DeckInteraction deck;
    private PlayerField player;

    private List<string> commands;

    private const int port = 8888;

    // Start is called before the first frame update
    void Awake()
    {
        deck = GameObject.Find("PlayerField").GetComponent<DeckInteraction>();
        player = GameObject.Find("PlayerField").GetComponent<PlayerField>();
        commands = new List<string>();
        InitializeRemoteIO();
        InitializeListenThread();
    }

    void Update(){

        if(commands.Count > 0){
            string line = commands[0];
            string[] args = line.Trim().Split('#');
            Debug.Log("Receiving: " + line);
            GameObject card;
            switch(args[0].ToUpper()){

                case "ATH":
                    deck.AddToHand(args[1]);
                    break;

                case "BOUNCE":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    card.GetComponent<CardAbility>().Bounce();
                    break;

                case "GOLD":
                    player.SetGold(int.Parse(args[1]));
                    break;

                case "ID":
                    deck.AssignIdToCardWithName(args[1], args[2]);
                    break;

                case "SUMMON":
                    player.SummonUnitWithId(args[1]);
                    break;

                case "PUTINDECK":
                    deck.ShuffleIntoDeck(GameObject.Find(args[1]));
                    break;

                case "TARGET":
                    GameState.targetting = true;
                    GameState.targettingCard = GameObject.Find(args[1]);
                    break;

                case "VALID_TARGET":
                    GameState.targetting = false;
                    GameState.targettingCard = null;
                    break;

                case "HEAL":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    card.GetComponent<CardData>().Heal(int.Parse(args[2]));
                    break;

                case "SEARCH":
                    deck.SearchCardFromDeckByName(args[1]);
                    break;

                case "CLOSE_SEARCH":
                    deck.CloseSearchWindow();
                    break;

                case "KILL":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    GameObject.Find("Discard").GetComponent<DiscardPile>().Discard(GameObject.Find(args[1]));
                    break;

                case "DAMAGE":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    card.GetComponent<CardData>().DealDamage(int.Parse(args[2]), bool.Parse(args[3]));
                    break;

                case "RESTORE":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    card.GetComponent<CardData>().Restore();
                    break;

                default:
                    break;

            }
            commands.RemoveAt(0);
        }
    }

    private void InitializeRemoteIO(){

        tcp = new TcpClient("localhost", port);
        NetworkStream stream = tcp.GetStream();
        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);
        writer.AutoFlush = true;

    }

    private void InitializeListenThread(){
        Thread thread = new Thread(new ThreadStart(ListenThread));
        thread.Start();
        Console.WriteLine("Connection established");
    }

    private void ListenThread(){

        bool connected = true;
        while(connected){

            string line = reader.ReadLine();
            if(line != null){

                commands.Add(line);

            }

        }

    }

    public void SendMessage(string message){
        Debug.Log(message);
        writer.WriteLine(message);
    }

}
