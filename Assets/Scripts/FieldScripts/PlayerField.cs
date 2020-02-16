﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerField : MonoBehaviour {

	public GameObject	field, 				// This player's field
	 									hand, 				// This player's hand
										deck, 				// This player's deck
										goldCounter,  // This player's gold counter
										discard;      // This player's discard pile
	public int startingGold = 25;		// The amount of gold this player starts with
	public int gold = 0;						// This player's gold quantity

	/*
	* Initiate the players game state
	*/
	public void Start(){

		GainGold(startingGold);
		this.transform.GetChild(0).SetParent(this.transform.parent);
		GameState.currentPlayer = gameObject;

	}

	/*
	* Give the player gold
	*/
	public void GainGold(int quantity){
		gold+=quantity;
		UpdateGoldText();
	}

	/*
	* Subtract gold from the player, IF they have enough gold to pay the cost
	*/
	public bool PayGold(int cost){
		if(cost <= gold){
			gold-=cost;
		}else{
			return false;
		}
		UpdateGoldText();
		return true;
	}

	/*
	*	Set the player's gold amount
	*/
	public void SetGold(int gold){
		this.gold = gold;
		UpdateGoldText();
	}

	/*
	*	Upgrade the gold counter for this player
	*/
	private void UpdateGoldText(){
		goldCounter.GetComponent<Text>().text = "" + gold;
	}

	/*
	*	Pay the wages for the Units this player has in play
	*/
	public void PayWages(){

		int wageMod = 1;
		int wageBonus = 0;
		int unitCount = 0;
		foreach(Transform unit in field.transform){
			wageMod *= unit.gameObject.GetComponent<CardData>().wageMod;
			wageBonus += unit.gameObject.GetComponent<CardData>().wageBonus;
			unitCount++;
		}
		int finalWages = (unitCount + wageBonus) * wageMod;
		PayGold(finalWages);

	}

}
