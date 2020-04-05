﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckBuilder : MonoBehaviour
{

    private Dictionary<string, int> deck;
    private GameObject deckView, cardSelect;

    private Text lordText, cardText;

    private int containsLord;

    private const string prefabPath = "Prefab/DeckBuilder/Cards";
    private const int deckSize = 25;

    // Start is called before the first frame update
    void Start()
    {
        deck = new Dictionary<string, int>();
        deckView = GameObject.Find("DeckView");
        cardSelect = GameObject.Find("CardSelection");
        lordText = GameObject.Find("LordNumberText").GetComponent<Text>();
        cardText = GameObject.Find("CardNumberText").GetComponent<Text>();
        LoadCardSelection();
        StartCoroutine(WaitThenFixScrollbar());
    }

    /*
    *   Adjust the scrollbar after a short period of time
    */
    IEnumerator WaitThenFixScrollbar(){
        yield return new WaitForSeconds(0.01f);
        GameObject.Find("SelectionScrollbar").GetComponent<Scrollbar>().value = 1;
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
        if(!card.CompareTag("Lord")){
            if(deckView.transform.childCount - containsLord >= deckSize){
                Debug.Log("You cannot have more than 25 cards in a deck (excluding the Lord card).");
                return;
            }
        }
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
                return;
            }
        }else{
            if(card.tag == "Lord"){
                containsLord = 1;
            }
            deck.Add(cardName, 1);
            CopyToDeckView(card);
        }
        UpdateNumbersText();

    }

    /*
    * Make the card visible in the deckview
    */
    private void CopyToDeckView(GameObject card){
        GameObject newCard = Instantiate(card, deckView.transform);
        newCard.GetComponent<Hoverable>().OnPointerExit(null);
        if(newCard.GetComponent<CardData>().cardType == CardData.Type.LORD){
            newCard.transform.SetSiblingIndex(0);
        }
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
                if(card.tag == "Lord"){
                    containsLord = 0;
                }
                deck.Remove(cardName);
            }
            Destroy(card);
            UpdateNumbersText();
        }

    }

    /*
    *   Update the decks card number text (Lords: x/1, Cards: y/25)
    */
    private void UpdateNumbersText(){
        cardText.color = Color.white;
        lordText.color = Color.white;
        lordText.text = "Lord Cards: " + containsLord + "/1";
        int totalCards = 0;
        foreach(KeyValuePair<string, int> entry in deck){
            totalCards += entry.Value;
        }
        int otherCards = totalCards - containsLord;
        cardText.text = "Cards: " + otherCards + "/25";
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

        if(!ValidateDeck()){
            return;
        }

        string[] deckCards = new string[deckSize+1];
        for(int i = 0; i < deckSize+1; i++){
            deckCards[i] = deckView.transform.GetChild(i).Find("Name").GetComponent<Text>().text;
        }

        DeckList deck = new DeckList(GetDeckName(), deckCards);
        DeckList.SaveDeck(deck);
        MainMenu();

    }

    /*
    *   Return true if the deck is legal, otherwise return false
    */
    public bool ValidateDeck(){

        bool deckValid = true, nameValid = true;

        int thisDeckSize = deckView.transform.childCount;
        cardText.color = Color.green;
        lordText.color = Color.green;
        if(thisDeckSize != 26){
            if(thisDeckSize - containsLord != 25){
                cardText.color = Color.red;
            }
            if(containsLord == 0){
                lordText.color = Color.red;
            }
            deckValid = false;
        }

        string deckName = GetDeckName().Trim();
        if(deckName.Length <= 0){
            GameObject input = GameObject.Find("DeckName");
            input.GetComponent<InputField>().text = "";
            input.transform.Find("Placeholder").GetComponent<Text>().color = new Color(1f, 0f, 0f, 1/2f);
            nameValid = false;
        }

        return (deckValid && nameValid);

    }

    /*
    *   Find the name of the deck
    */
    private string GetDeckName(){
        GameObject input = GameObject.Find("DeckName");
        GameObject textField = input.transform.Find("Text").gameObject;
        return textField.GetComponent<Text>().text;
    }

    /*
    *   Return to the main menu
    */
    public void MainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void MakePlaceholderTextWhite(){
        GameObject input = GameObject.Find("DeckName");
        input.transform.Find("Placeholder").GetComponent<Text>().color = new Color(1f, 1f, 1f, 1/2f);
    }


}
