using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCardStats : MonoBehaviour {

	private CardData data; // This cards CardData script

	/*
	*	Triggers on start
	*/
	void Start () {
		data = GetComponent<CardData>();
		GetAndSetCost();
		if(data.cardType == CardData.Type.MINION){
			GetAndSetStrength();
			GetAndSetHealth();
		}
	}

	/*
	* Works out the stats of this card, and updates the card text to match
	*/
	private void GetAndSetCost(){
		int cost = data.cost;
		transform.Find("Coin Counter/Text").GetComponent<Text>().text = "" + cost;
	}
	private void GetAndSetStrength(){
		int strength = data.strength;
		transform.Find("Strength/Text").GetComponent<Text>().text = "" + strength;
	}
	private void GetAndSetHealth(){
		int health = data.health;
		transform.Find("Health/Text").GetComponent<Text>().text = "" + health;
	}

}
