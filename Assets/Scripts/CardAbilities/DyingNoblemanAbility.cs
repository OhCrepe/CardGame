using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingNoblemanAbility : CardAbility {

		public int goldGained;

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
		*	Ability that triggers on death.
		*	Gains 6 gold. It doesn't matter how to unit dies in the case, so we override OnKillAbility()
		*/
		public override void OnKillAbility(bool combat){
			player.GetComponent<PlayerField>().GainGold(goldGained);
		}

		/*
		* Validate that the target of this ability is correct
		*/
		public override bool ValidateTarget(GameObject card){
			return true;
		}

}
