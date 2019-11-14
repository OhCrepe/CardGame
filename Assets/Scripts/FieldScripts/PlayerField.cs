using System.Collections;
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

	}

	/*
	* Give the player gold
	*/
	public void GainGold(int quantity){
		gold+=quantity;
		UpgradeGoldText();
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
		UpgradeGoldText();
		return true;
	}

	/*
	*	Upgrade the gold counter for this player
	*/
	private void UpgradeGoldText(){
		goldCounter.GetComponent<Text>().text = "" + gold;
	}

}
