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

	public void SetHealth(int health, int maxHealth){
		Text healthText = transform.Find("Health/Text").GetComponent<Text>();
		if(health < maxHealth){
			healthText.color = Color.red;
		}
		if(health > maxHealth){
			healthText.color = Color.green;
		}
		if(health == maxHealth){
			healthText.color = Color.black;
		}
		healthText.text = "" + health;

	}

	public void SetStrength(int strength, int maxStrength){
		Text strengthText = transform.Find("Strength/Text").GetComponent<Text>();
		if(strength < maxStrength){
			strengthText.color = Color.red;
		}
		if(strength > maxStrength){
			strengthText.color = Color.green;
		}
		if(strength == maxStrength){
			strengthText.color = Color.black;
		}
		strengthText.text = "" + strength;

	}

}
