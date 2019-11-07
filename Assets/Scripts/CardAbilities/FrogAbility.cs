using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAbility : CardAbility {

	/*
	* The ability of this card that triggers on summon
	*/
	public override void OnHire(){
		CheckUtility();
	}

	/*
	* Ability of this card that triggers when the ability button is clicked
	* In this case, we pay 2 gold and then return this card to the hand from the field
	*/
	public override void OnFieldTrigger(){
		if(player.GetComponent<PlayerField>().PayGold(fieldTriggerCost)){
			this.gameObject.transform.SetParent(player.GetComponent<PlayerField>().hand.transform);
		}
	}

	/*
	* Validate that the target of this ability is correct
	*/
	public override bool ValidateTarget(GameObject card){
		return true;
	}

}
