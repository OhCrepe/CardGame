using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour {

	/*
	* Stores information about this card such as cost
	*/

	public int cost, strength, health; // Cost to hire this warrior/activate this utility
	public enum Type { MINION, UTILITY }; // Whether this card is a utility or a minion
	public Type cardType;

	private int currentHealth;
	private int currentStrength;

	void Awake(){
		currentHealth = health;
		currentStrength = strength;
	}

	public void DealDamage(int damage){
		currentHealth-=damage;
		GetComponent<GetCardStats>().SetHealth(currentHealth, health);
		if(currentHealth <= 0){
			GetComponent<CardAbility>().Kill();
		}
	}

	public void DealDamageTo(GameObject card){
		card.GetComponent<CardData>().DealDamage(currentStrength);
	}

}
