using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class DeckList
{

    public string deckName;
    public string[] deckList;
    private const string deckLocations = "/decks/";
    private const string fileExtension = ".dek";

    public DeckList(){

    }

    public DeckList(string name, string[] list){

        deckName = name;
        deckList = list;

    }

    /*
    *   Save the deck to a binary formatted file.
    *   //TODO will likely have to make this more robust in the future
    */
    public static void SaveDeck(DeckList deck){

        BinaryFormatter formatter = new BinaryFormatter();

        string filepath = GetDeckPath(deck.deckName);
        FileStream file = File.Create(filepath);

        Debug.Log("Saving - " + deck.deckList.Length);

        formatter.Serialize(file, deck);
        file.Close();

    }

    /*
    *   Load the deck from a binary formatted file, given the name of the deck.
    */
    public static DeckList LoadDeck(string name){

        string filepath = GetDeckPath(name);

        if(!File.Exists(filepath)){

            Debug.Log("Error loading deck - " + filepath);
            return null;

        }else{

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(filepath, FileMode.Open);
            DeckList deck = (DeckList)formatter.Deserialize(file);
            file.Close();
            return deck;

        }

    }

    /*
    *   Get the location of the deck stored in memory
    */
    private static string GetDeckPath(string name){
        string location = Application.persistentDataPath + deckLocations;
        Directory.CreateDirectory(location);
        Debug.Log(location);
        return location + name + fileExtension;
    }

    /*
    *   Returns a list of all decks this player has created
    */
    public static List<string> GetAllDeckNames(){
        string location = Application.persistentDataPath + deckLocations;
        Directory.CreateDirectory(location);
        DirectoryInfo dirInfo = new DirectoryInfo(location);
        FileInfo[] info = dirInfo.GetFiles("*.dek");

        List<string> names = new List<string>();
        foreach(FileInfo file in info){
            string name = file.ToString();
            int lastSlash = name.LastIndexOf('\\');
            int lastDot = name.LastIndexOf('.');
            name = name.Substring(lastSlash + 1, lastDot - lastSlash - 1);
            Debug.Log(name);
            names.Add(name);
        }
        return names;
    }

}
