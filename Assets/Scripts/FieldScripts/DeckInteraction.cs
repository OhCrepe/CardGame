using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DeckInteraction : MonoBehaviour {

	public GameObject playerHand; // Hand of the player this deck belongs to
	public GameObject deckObject; // The deck that we're using
	public int handSize;

	/*
	With the deck linked list, the first element at index 0 is the card at the
	TOP of the deck, the element at index size-1 is the card at the BOTTOM.
	*/

	private List<GameObject> deck = new List<GameObject>();

	/*
	* Triggers at start of game
	*/
	void Start () {
		LoadDeck();
		InitDeck();
		CheckForDeck();
		AddToHand("Dying Nobleman");
	}

	/*
	*	Load the deck from memory
	*	// TODO at the moment deck uses "sample.dek" by default
	*/
	private void LoadDeck(){

		DeckList deck = DeckList.LoadDeck("Sample");
		if(deck == null){
			return;
		}
		foreach(string name in deck.deckList){
			if(CardMap.GetCardType(name) != "Lord"){
				CardMap.InstantiateToZone(name, deckObject.transform);
			}else{
				CardMap.InstantiateToZone(name, GameObject.Find("Lord").transform);
			}
		}


	}

	/*
	*	Puts the cards into the deck linkedlist and shuffles it
	*/
	public void InitDeck(){

		for(int i = deckObject.transform.childCount - 1; i > 0; i--){

			AddToTop(deckObject.transform.GetChild(i).gameObject);

		}

		foreach(GameObject go in deck){
			Debug.Log(go.name);
		}
		ShuffleDeck();
		Debug.Log("The deck has " + deck.Count + " cards in it");

		for(int i = 0; i < handSize; i++){
			DrawCard();
		}

	}

	/*
	* Draws a card from the top of the deck if the deck isn't empty
	*/
	public GameObject DrawCard(){

		if(deck.Count == 0){
			return null;
		}
		GameObject drawnCard = deck[0];
		MoveCardToHand(drawnCard);
		CheckForDeck();
		return drawnCard;

	}

	/*
	*	Adds a copy of a card that is in the deck to the hand with the specified name
	*/
	public void AddToHand(string name){

		foreach(GameObject card in deck){
			if(card.transform.Find("Name").GetComponent<Text>().text == name){
				AddToHand(card);
				return;
			}
		}

		Debug.Log("Card with name: " + name + " could not be found");

	}

	/*
	*	Adds a specific card that is in the deck into the hand
	*/
	public void AddToHand(GameObject card){

		if(deck.Contains(card)){
			MoveCardToHand(card);
		}
		CheckForDeck();
		ShuffleDeck();

	}

	/*
	*	Helper function to handle common functionality between searching and drawing
	*/
	private void MoveCardToHand(GameObject card){

		deck.Remove(card);
		card.transform.SetParent(playerHand.transform);
		//RpcMoveCardToParent(card, playerHand);
		card.GetComponent<Draggable>().parentToReturnTo = playerHand.transform;
		card.SetActive(true);
	}

	/*
	* Puts a card on top of thte deck and then shuffles it
	*/
	public void ShuffleIntoDeck(GameObject card){
		if(!deck.Contains(card)){
			AddToTop(card);
		}
		ShuffleDeck();
	}


	/*
	* Places a card on top of the deck
	*/
	public void AddToTop(GameObject card){
		if(deck.Contains(card)){
			deck.Remove(card);
		} else {
			card.transform.SetParent(deckObject.transform);
			card.SetActive(false);
			CheckForDeck();
		}
		deck.Insert(0, card);
	}

	/*
	* Puts a card on the bottom of the deck. Card can be from in or out of the deck.
	*/
	public void AddToBottom(GameObject card){
		if(deck.Contains(card)){
			deck.Remove(card);
		} else {
			card.transform.SetParent(deckObject.transform);
			card.SetActive(false);
			CheckForDeck();
		}
		deck.Insert(deck.Count, card);

	}

	/*
	*	Randomise the order of the cards in the deck
	*/
	private void ShuffleDeck(){

		List<GameObject> newDeck = new List<GameObject>();
		for(int i = deck.Count-1; i >= 0; i--){
			int cardIndex = (int)(UnityEngine.Random.Range(0, 1f)*i);
			newDeck.Insert(0, deck[cardIndex]);
			deck.RemoveAt(cardIndex);
		}
		deck = newDeck;

	}

	/*
	*	Will disable the card back if the deck is empty (or enable othewise)
	*/
	private void CheckForDeck(){
		if(deck.Count == 0){
			SetDeckVisibility(false);
			return;
		}
		SetDeckVisibility(true);
	}
	private void SetDeckVisibility(bool visible){
		deckObject.transform.GetChild(0).gameObject.SetActive(visible);
	}

	public int DeckCount(){
		return deck.Count;
	}

}
