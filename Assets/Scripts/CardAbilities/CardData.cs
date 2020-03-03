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
	public int wageMod, wageBonus;

  	private int currentHealth;
	private int currentStrength;
	private string id;

	/*
	*	Initialize the card
	*/
	void Awake(){
		currentHealth = health;
		currentStrength = strength;
		id = null;
	}

	/*
	*	Restore the given health to the card
	*/
	public void Heal(int healthGained){
		currentHealth += healthGained;
		if(currentHealth > health){
			currentHealth = health;
		}
		GetComponent<GetCardStats>().SetHealth(currentHealth, health);
	}

	/*
	*	Deal the given health to the card
	*/
	public void DealDamage(int damage){
		currentHealth-=damage;
		GetComponent<GetCardStats>().SetHealth(currentHealth, health);

	}

	/*
	*	Restore the unit to full health and strength
	*/
	public void Restore(){
		currentHealth = health;
		GetComponent<GetCardStats>().SetHealth(currentHealth, health);
		currentStrength = strength;
	}

	/*
	*	Set the id of the unit if it is not null
	*/
	public void SetId(string id){
		if(IsIdNull()){
			this.id = id;
		}
	}

	/*
	*	Check if the id is null
	*/
	public bool IsIdNull(){
		return id == null;
	}

	/*
	*	Return the id
	*/
	public string GetId(){
		return id;
	}

}
