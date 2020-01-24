using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMedicAbility : CardAbility
{

    public int healthGained;

    /*
    * Ability of this card that triggers when the ability button is clicked
	* In this case, we pay 1 gold and then restore 2 health to a unit
    */
	public override void OnFieldTrigger(){
        if(!oncePerTurnUsed){
        	if(player.GetComponent<PlayerField>().PayGold(fieldTriggerCost)){
                oncePerTurnUsed = true;
                StartCoroutine(WaitForTarget());
        	}
        }
    }

    /*
    * Ability of this card that triggers when the a target is selected for it's ability
    * In this case, we pay 1 gold and then restore 2 health to a unit
    */
    public override void OnTargetSelect(GameObject card){
        card.GetComponent<CardData>().Heal(healthGained);
    }

    /*
    * Validate that the target of this ability is correct
    */
    public override bool ValidateTarget(GameObject card){

        if(card.tag != "Card"){
            return false;
        }
        if(card.transform.parent.GetComponent<Dropzone>().zoneType != Dropzone.Zone.FIELD){
            return false;
        }
        if(card.GetComponent<CardData>().cardType != CardData.Type.MINION){
            return false;
        }
        Debug.Log("Valid target");
        return true;

    }

}
