using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityButtonToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private GameObject abilityButton, // The button that we press to trigger this cards ability
						attackButton; // The button that we press to trigger this cards attack

    // Start is called before the first frame update
    void Awake()
    {
        if(GetComponent<CardData>().cardType == CardData.Type.MINION){
            abilityButton = transform.Find("Ability Button").gameObject;
            attackButton = transform.Find("Attack Button").gameObject;
        }
    }

    /*
	* Check whether an ability button exists (defined by whether this is a minion or a utility), and
	* set it's activeness as to which of the two it is.
	*/
	private void ConfigureAbilityButton(bool active){

        CardAbility ability = GetComponent<CardAbility>();
        if(transform.parent.name.Contains("Enemy")) return;
		if(this.gameObject.GetComponent<CardData>().cardType == CardData.Type.UTILITY){
			return;
		}

        if(ability.hasAttacked){
            attackButton.SetActive(false);
        }
		else attackButton.SetActive(active);

        if(ability.oncePerTurnUsed || !ability.hasTriggerAbility){
            abilityButton.SetActive(false);
        }
        else abilityButton.SetActive(active);

    }

    /*
	* Detects when the mouse hovers over the card
	*/
	public void OnPointerEnter(PointerEventData eventData){

		if(!GameState.dragging){

			if(this.transform.parent.GetComponent<Dropzone>().zoneType == Dropzone.Zone.FIELD){
				ConfigureAbilityButton(true);
			}

		}

	}

    /*
    * Detects when the mouse stops hovering over the card
    */
    public void OnPointerExit(PointerEventData eventData){
        ConfigureAbilityButton(false);
    }

}
