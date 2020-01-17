using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckBuilder : MonoBehaviour
{

    private Dictionary<string, int> deck;
    private GameObject deckView, cardSelect;

    private const string prefabPath = "Prefab/DeckBuilder/Cards";

    // Start is called before the first frame update
    void Start()
    {
        deck = new Dictionary<string, int>();
        deckView = GameObject.Find("DeckView").gameObject;
        cardSelect = GameObject.Find("CardSelection").gameObject;
        LoadCardSelection();
    }

    /*
    *   Load all cards in the game
    */
    private void LoadCardSelection(){

        Object[] cardArray = Resources.LoadAll(prefabPath, typeof(GameObject));
        foreach(GameObject cardObject in cardArray){

            GameObject card = (GameObject)cardObject;
            Instantiate(card, cardSelect.transform);

        }
    }

    /*
    *   Add a copy of the provided card to the deck
    */
    public void AddToDeck(GameObject card){

        string cardName = GetCardName(card);
        Debug.Log("Adding " + cardName + " to deck");
        if(deck.ContainsKey(cardName)){
            if(card.tag == "Lord"){
                Debug.Log("You can't have more than 1 copy of a Lord card in your deck!");
                return;
            }
            if(deck[cardName] < 3){
                deck[cardName] = deck[cardName] + 1;
                CopyToDeckView(card);
            }else{
                Debug.Log("You can't have more than 3 copies of a card in your deck!");
            }
        }else{
            deck.Add(cardName, 1);
            CopyToDeckView(card);
        }

    }

    /*
    * Make the card visible in the deckview
    */
    private void CopyToDeckView(GameObject card){
        GameObject newCard = Instantiate(card, deckView.transform);
        newCard.GetComponent<Hoverable>().OnPointerExit(null);
    }

    /*
    *   Remove the selected card from the deck
    */
    public void RemoveFromDeck(GameObject card){

        string cardName = GetCardName(card);
        Debug.Log("Removing " + cardName + " from deck");
        if(deck.ContainsKey(cardName)){
            if(deck[cardName] > 1){
                deck[cardName] = deck[cardName] - 1;
            }else{
                deck.Remove(cardName);
            }
            Destroy(card);
        }

    }

    /*
    *   Find the name of the given card
    */
    private string GetCardName(GameObject card){
        return card.transform.Find("Name").GetComponent<Text>().text;
    }

    /*
    *   Save the deck to a file that can be loaded
    */
    public void SaveDeck(){

        string deckName = GetDeckName();
        if(deckName.Length <= 0){
            // TODO Tell the user that the deck name is invalid
            return;
        }
        Debug.Log("valid");

        int deckSize = deckView.transform.childCount;

        string[] deckCards = new string[deckSize];
        for(int i = 0; i < deckSize; i++){
            deckCards[i] = deckView.transform.GetChild(i).Find("Name").GetComponent<Text>().text;
        }

        DeckList deck = new DeckList(deckName, deckCards);
        DeckList.SaveDeck(deck);

    }

    /*
    *   Find the name of the deck
    */
    private string GetDeckName(){
        GameObject input = GameObject.Find("DeckName");
        GameObject textField = input.transform.Find("Text").gameObject;
        return textField.GetComponent<Text>().text;
    }

    public void MainMenu(){
        SceneManager.LoadScene("MainMenu");
    }


}
