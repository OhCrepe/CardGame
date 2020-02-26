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
		//player.GetComponent<PlayerField>().GainGold(goldGained);
		//CheckUtility();
	}

	/*
	* Validate that the target of this ability is correct
	*/
	public override bool ValidateTarget(GameObject card){
		return true;
	}

}
