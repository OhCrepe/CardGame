using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogElderAbility : CardAbility
{

    /*
    * Ability of this card that triggers when the ability button is clicked
    * In this case, we pay 1 gold and then restore 2 health to a unit
    */
    public override void OnFieldTrigger(){

        if(player.GetComponent<PlayerField>().PayGold(fieldTriggerCost)){

            Bounce();
            player.GetComponent<DeckInteraction>().SearchCardFromDeckByName("Frog");
            StartCoroutine(WaitForTarget());

        }

    }

    /*
	* Ability of this card that triggers when the a target is selected for it's ability
	* In this case discard that card, and give the player gold equal to it's cost
	*/
	public override void OnTargetSelect(GameObject card){
        DeckInteraction deck = player.GetComponent<DeckInteraction>();
		deck.AddToHand(card);
        deck.CloseSearchWindow();
	}

    /*
	*	Check that black market can resolve. Makes sure there's a card in hand to shuffle
	* into the deck, and that there'll be 2 card in deck after the card is shuffled in.
	*/
	protected override bool ActivationRequirementsMet(){
		DeckInteraction deck = player.GetComponent<DeckInteraction>();
		return deck.CheckForTargetByName("Frog");
	}

    /*
    * Validate that the target of this ability is correct
    */
    public override bool ValidateTarget(GameObject card){

        if(card.tag != "Card"){
            return false;
        }
        if(card.transform.parent.GetComponent<Dropzone>().zoneType != Dropzone.Zone.SEARCH){
            return false;
        }
        Debug.Log("Valid target");
        return true;

    }

}
