using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NetworkConnection : MonoBehaviour, IDisposable
{

    public GameObject menuButton;

    private StreamReader reader;
    private StreamWriter writer;
    private TcpClient tcp;

    private DeckInteraction deck;
    private PlayerField player, opponent;
    private GameStateReader stateReader;

    private GameObject blank;

    private List<string> commands;

    private const string ip = "93.186.159.174";
    private const int port = 8888;

    // Initialize the network connection
    void Awake()
    {
        deck = GameObject.Find("PlayerField").GetComponent<DeckInteraction>();
        player = GameObject.Find("PlayerField").GetComponent<PlayerField>();
        opponent = GameObject.Find("OpponentField").GetComponent<PlayerField>();
        stateReader = GameObject.Find("GameStateReader").GetComponent<GameStateReader>();
        blank = (GameObject)Resources.Load("Prefab/Blank", typeof(GameObject));
        commands = new List<string>();
        InitializeRemoteIO();
        InitializeListenThread();
    }


    /*
    *   Initialize the stream reader/writer and tcp client
    */
    private void InitializeRemoteIO(){

        if(GameState.localServer){
            tcp = new TcpClient("localhost", port);
        }
        else{
            tcp = new TcpClient(ip, port);
        }
        NetworkStream stream = tcp.GetStream();
        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);
        writer.AutoFlush = true;
        Debug.Log("Initializing Connection");

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
                    card = CardMap.cardsInGame[args[1]];
                    deck.AddToHand(card);
                    break;

                // Instantiate a blank card in the opponents hand
                case "ATH_OPP":
                    Instantiate(blank, opponent.hand.transform);
                    break;

                // Mark this card as having attacked
                case "ATK_USED":
                    card = CardMap.cardsInGame[args[1]];
                    if(card == null) break;
                    card.GetComponent<CardAbility>().hasAttacked = true;
                    card.GetComponent<AbilityButtonToggle>().OnPointerEnter(null);
                    break;

                // Bounce a card to hand
                case "BOUNCE":
                    card = CardMap.cardsInGame[args[1]];
                    if(card == null) break;
                    card.GetComponent<CardAbility>().Bounce();
                    break;

                // Bounce an opponent's card to hand
                case "BOUNCE_OPP":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    Destroy(card);
                    Instantiate(blank, opponent.hand.transform);
                    break;

                // Tell the client they can now declare attacks
                case "CAN_ATTACK":
                    GameState.canAttack = true;
                    break;

                // Close the search window
                case "CLOSE_SEARCH":
                    deck.CloseSearchWindow();
                    break;

                // Deal damage to a card
                case "DAMAGE":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    stateReader.ChangeText(card.transform.Find("Name").GetComponent<Text>().text + " took "  + args[2]  + " damage!");
                    card.GetComponent<CardData>().DealDamage(int.Parse(args[2]));
                    break;

                // Change the value of a player's gold
                case "GOLD":
                    player.SetGold(int.Parse(args[1]));
                    break;

                    // Change the value of a player's gold
                case "GOLD_OPP":
                    opponent.SetGold(int.Parse(args[1]));
                    break;

                // Heal a unit
                case "HEAL":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    stateReader.ChangeText(card.transform.Find("Name").GetComponent<Text>().text + " was healed!");
                    card.GetComponent<CardData>().Heal(int.Parse(args[2]));
                    break;

                // Assign an id to a card
                case "ID":
                    deck.AssignIdToCardWithName(args[1], args[2]);
                    break;

                // Invalid activation, put that card back on the field
                case "INVALID":
                    card = CardMap.cardsInGame[args[1]];
                    if(card == null) break;
                    card.GetComponent<CardAbility>().Bounce();
                    stateReader.ChangeText("You can't activate that card right now!");
                    break;

                // Kill a card
                case "KILL":
                    card = CardMap.cardsInGame[args[1]];
                    if(card == null) break;
                    stateReader.ChangeText(card.transform.Find("Name").GetComponent<Text>().text + " was killed!");
                    GameObject.Find("Discard").GetComponent<DiscardPile>().Discard(GameObject.Find(args[1]));
                    break;

                // Kill an opponent's card
                case "KILL_OPP":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    stateReader.ChangeText(card.transform.Find("Name").GetComponent<Text>().text + " was killed!");
                    Destroy(card, FloatingTextController.floatingTextLength);
                    break;

                // Spawn the opponent's lord card
                case "LORD_OPP":
                    card = CardMap.InstantiateToZone(args[1], GameObject.Find("Enemy Lord").transform);
                    card.name = args[2];
                    break;

                // We lose
                case "LOSE":
                    GameState.gameOver = true;
                    stateReader.ChangeText("Defeat!");
                    Disconnect();
                    menuButton.SetActive(true);
                    break;

                case "OPT_USED":
                    card = CardMap.cardsInGame[args[1]];
                    if(card == null) break;
                    card.GetComponent<CardAbility>().oncePerTurnUsed = true;
                    card.GetComponent<AbilityButtonToggle>().OnPointerEnter(null);
                    break;

                // Put a card into the deck
                case "PUTINDECK":
                    card = CardMap.cardsInGame[args[1]];
                    if(card == null)break;
                    deck.ShuffleIntoDeck(card);
                    break;

                case "RESET_OPT":
                    card = CardMap.cardsInGame[args[1]];
                    if(card == null) break;
                    card.GetComponent<CardAbility>().ResetOncePerTurns();
                    break;

                // Restore a cards stats
                case "RESTORE":
                    card = CardMap.cardsInGame[args[1]];
                    if(card == null) break;
                    card.GetComponent<CardData>().Restore();
                    break;

                // Remove a card from the opponents hand
                case "RFH":
                    if(opponent.hand.transform.childCount == 0) break;
                    Destroy(opponent.hand.transform.GetChild(0).gameObject);
                    break;

                // Search for a card
                case "SEARCH":
                    deck.SearchCardFromDeckByName(args[1]);
                    break;

                // Buff this cards strength
                case "STRENGTH_UP":
                    card = GameObject.Find(args[1]);
                    if(card == null) break;
                    card.GetComponent<CardData>().ChangeStrength(int.Parse(args[2]));
                    stateReader.ChangeText(card.transform.Find("Name").GetComponent<Text>().text + " has increased strength!");
                    break;

                // Summon a card
                case "SUMMON":
                    player.SummonUnitWithId(args[1]);
                    stateReader.ChangeText("You summoned a unit!");
                    break;

                // Summon a card
                case "SUMMON_OPP":
                    card = CardMap.InstantiateToZone(args[1], opponent.field.transform);
                    card.name = args[2];
                    card.GetComponent<CardData>().SetId(args[2]);
                    stateReader.ChangeText("Your opponent summoned " + args[1] + "!");
                    break;

                // Target a card
                case "TARGET":
                    GameState.targetting = true;
                    card = CardMap.cardsInGame[args[1]];
                    GameState.targettingCard = card;
                    stateReader.ChangeText("Select a target for the effect of " + card.transform.Find("Name").GetComponent<Text>().text);
                    break;

                // The given target is valid
                case "VALID_TARGET":
                    GameState.targetting = false;
                    GameState.targettingCard = null;
                    stateReader.ChangeText("Turn player is deciding what to do...");
                    break;

                // We win
                case "WIN":
                    GameState.gameOver = true;
                    stateReader.ChangeText("Victory!");
                    Disconnect();
                    menuButton.SetActive(true);
                    break;

                // My turn
                case "YOUR_OPPONENTS_TURN":
                    stateReader.ChangeText("It is your opponent's turn!");
                    break;

                // My turn
                case "YOUR_TURN":
                    stateReader.ChangeText("It is your turn!");
                    break;

                default:
                    break;

            }

            commands.RemoveAt(0);


        }

        if(!tcp.Connected && !GameState.gameOver){
            stateReader.ChangeText("You've been disconnected from the server.");
            menuButton.SetActive(true);
        }

    }

    /*
    *   Start the thread that listens to the server
    */
    private void InitializeListenThread(){
        Debug.Log("Starting listen thread");
        Thread thread = new Thread(new ThreadStart(ListenThread));
        thread.Start();
        Debug.Log("Connection established");
    }

    /*
    *   Listen for commands given by the server and add them to the queue
    */
    private void ListenThread(){

        while(tcp.Connected){
            try{
                string line = reader.ReadLine();
                if(line != null){

                    commands.Add(line);

                }
            }catch(ObjectDisposedException e){
                Disconnect();
            }catch(Exception e){
                Disconnect();
            }
        }

    }

    /*
    *   Send a message to the server
    */
    public void SendMessage(string message){
        if(GameState.gameOver) return;
        if(!tcp.Connected) return;
        Debug.Log("Sending: " + message);
        writer.WriteLine(message);
    }

    public void Disconnect(){Dispose();}
    /*
     * Close the connection
     */
    public void Dispose()
    {
        if(tcp.Connected){
            tcp.GetStream().Close();
            tcp.Close();
        }
    }

    void OnApplicationQuit(){ Dispose(); }
    void OnDestroy(){ Dispose(); }

}
