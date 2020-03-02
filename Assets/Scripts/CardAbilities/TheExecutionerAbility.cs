using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheExecutionerAbility : CardAbility
{
    // Start is called before the first frame update
    void Start()
    {
        hasEndOfTurnAbility = true;
    }

    // Ability that triggers on the field when clicked
  	public override void EndOfTurnAbility(){
        
        /*
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach(GameObject card in cards){
            if(card.GetComponent<Draggable>().parentToReturnTo.GetComponent<Dropzone>().zoneType == Dropzone.Zone.FIELD){
                if(card.GetComponent<CardData>().health == 1){
                    card.GetComponent<CardAbility>().Kill(false);
                }
            }
        }
        */

  	}

}
