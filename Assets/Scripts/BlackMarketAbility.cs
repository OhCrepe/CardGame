using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMarketAbility : CardAbility {

	/*
	* The ability of this card that triggers on summon
	* In this case, we discard a card and gain it's cost as gold
	*/
	public override void OnHire(){
		StartCoroutine(WaitForTarget());
		//CheckUtility();
	}

	/*
	* Ability of this card that triggers when the ability button is clicked
	* In this case, we do nothing
	*/
	public override void OnFieldTrigger(){
		//Do nothing
	}

	/*
	* Ability of this card that triggers when the a target is selected for it's ability
	* In this case discard that card, and give the player gold equal to it's cost
	*/
	public override void OnTargetSelect(GameObject card){
		Debug.Log(card.name + " selected, gaining " + card.GetComponent<CardData>().cost + " gold.");
		player.GetComponent<PlayerField>().discard.GetComponent<DiscardPile>().Discard(card);
		player.GetComponent<PlayerField>().GainGold(card.GetComponent<CardData>().cost);
		CheckUtility();
	}

	/*
	* Validate that the target of this ability is correct
	*/
	public override bool ValidateTarget(GameObject card){

		if(card.tag != "Card"){
			return false;
		}
		if(card.transform.parent.GetComponent<Dropzone>().zoneType != Dropzone.Zone.HAND){
			return false;
		}
		Debug.Log("Valid target");
		return true;

	}

}
