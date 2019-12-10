using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilder : MonoBehaviour
{

    private Dictionary<string, int> deck;
    private GameObject deckView, select;

    // Start is called before the first frame update
    void Start()
    {
        deck = new Dictionary<string, int>();
        deckView = GameObject.Find("DeckView").gameObject;
        select = GameObject.Find("CardSelection").gameObject;
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


}
