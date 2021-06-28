using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithdrawalAbility : CardAbility {

	public int goldGained;

	/*
	* The ability of this card that triggers on summon
	* In this case, we give the player who played it 2 gold
	*/
	public override void OnHire(){
		player.GetComponent<PlayerField>().GainGold(goldGained);
		CheckUtility();
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
	* in this case do nothing
	*/
	public override void OnTargetSelect(GameObject card){
		//Do nothing
	}

	/*
	* Validate that the target of this ability is correct
	*/
	public override bool ValidateTarget(GameObject card){
		return true;
	}

}
