using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloatedBodyScript : CardAbility
{

    public int damageDealt;

    /*
    *	Ability that triggers on death.
    *	Gains 6 gold. It doesn't matter how to unit dies in the case, so we override OnKillAbility()
    */
    public override void OnKillAbility(bool combat){
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach(GameObject card in cards){
            CardData data = card.GetComponent<CardData>();
            if(data.cardType == CardData.Type.MINION){
                if(card.transform.parent.GetComponent<Dropzone>().zoneType == Dropzone.Zone.FIELD){
                    data.DealDamage(damageDealt, false);
                }
            }
        }
    }

}
