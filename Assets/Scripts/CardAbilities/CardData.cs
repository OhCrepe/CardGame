using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour {

	/*
	* Stores information about this card such as cost
	*/

	public int cost, strength, health; // Cost to hire this warrior/activate this utility
	public enum Type { LORD, MINION, UTILITY }; // Whether this card is a utility or a minion
	public Type cardType;
	public int wageMod, wageBonus;

  	private int currentHealth;
	private int currentStrength;
	private string id;
	private Color orange;

	/*
	*	Initialize the card
	*/
	void Awake(){
		id = null;
		if(cardType == Type.LORD) return;
		currentHealth = health;
		currentStrength = strength;
		orange = new Color(248/255f, 140/255f, 0f, 1f);
	}

	/*
	*	Restore the given health to the card
	*/
	public void Heal(int healthGained){

		if(cardType == Type.LORD) return;

		currentHealth += healthGained;
		if(currentHealth > health){
			currentHealth = health;
		}
		GetComponent<GetCardStats>().SetHealth(currentHealth, health);
		string floatingText = "+" + healthGained;
		FloatingTextController.CreateFloatingText(floatingText, Color.green, transform.Find("Health"));
	}


	/*
	*	Restore the given health to the card
	*/
	public void ChangeStrength(int changeBy){

		if(cardType == Type.LORD) return;

		currentStrength += changeBy;
		GetComponent<GetCardStats>().SetStrength(currentStrength, strength);
		string floatingText = ""+ changeBy;
		if(changeBy > 0){
			floatingText = "+" + changeBy;
		}
		FloatingTextController.CreateFloatingText(floatingText, orange, transform.Find("Strength"));
	}


	/*
	*	Deal the given health to the card
	*/
	public void DealDamage(int damage){

		if(cardType == Type.LORD) return;

		currentHealth-=damage;
		GetComponent<GetCardStats>().SetHealth(currentHealth, health);
		string floatingText = "-"  + damage;
		FloatingTextController.CreateFloatingText(floatingText, Color.red, transform.Find("Health"));
	}

	/*
	*	Restore the unit to full health and strength
	*/
	public void Restore(){

		if(cardType == Type.LORD) return;

		currentHealth = health;
		GetComponent<GetCardStats>().SetHealth(currentHealth, health);
		currentStrength = strength;
		GetComponent<GetCardStats>().SetHealth(currentStrength, strength);

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
