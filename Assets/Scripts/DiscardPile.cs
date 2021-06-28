using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour {

	/*
	With the pile linked list, the first element at index 0 is the card that was
	first discards, the element at index size-1 is the last.
	*/
	private List<GameObject> pile = new List<GameObject>();

	/*
	* Discard a card into the discard pile. Disable it and add it to "pile"
	*/
	public void Discard(GameObject card){

		if(pile.Contains(card)){
			return;
		}

		card.GetComponent<Draggable>().parentToReturnTo = this.transform;

		card.transform.SetParent(this.transform);
		pile.Insert(pile.Count, card);
		card.SetActive(false);
		Debug.Log("Sorted");

	}

}
