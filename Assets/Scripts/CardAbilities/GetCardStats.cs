using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCardStats : MonoBehaviour {

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
		int cost = transform.gameObject.GetComponent<CardData>().cost;
		transform.Find("Coin Counter/Text").GetComponent<Text>().text = "" + cost;
	}

}
