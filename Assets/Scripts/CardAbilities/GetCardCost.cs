using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetCardCost : MonoBehaviour {

	/*
	*	Triggers on start
	*/
	void Start () {
		GetAndSetCost();
	}

	/*
	* Works out the cost to activate this card, and updates the gold cost text to match
	*/
	private void GetAndSetCost(){
		int cost = this.gameObject.transform.parent.parent.gameObject.GetComponent<CardData>().cost;
		this.gameObject.GetComponent<Text>().text = "" + cost;
	}

}
