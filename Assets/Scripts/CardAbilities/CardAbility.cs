using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardAbility : MonoBehaviour {

	public GameObject player; // The player this card belongs to
	public int fieldTriggerCost; //The cost to trigger the field effect of this card
	public bool hasTriggerAbility;

	public abstract void OnHire(); // Ability that triggers when hired
	public abstract void OnFieldTrigger(); // Ability that triggers on the field when clicked
	public abstract void OnKillAbility();
	public abstract void OnTargetSelect(GameObject card); // What to do when a target is selected for this cards ability
	public abstract bool ValidateTarget(GameObject card); //Validate that the target for this ability is valid

	private GameObject target = null;

	public void CheckUtility(){
		if(this.transform.GetComponent<CardData>().cardType == CardData.Type.UTILITY){
			Kill();
		}
	}

	/*
	*	Wait for the user to select a card
	*/
	public IEnumerator WaitForTarget(){

		bool selected = false;
		GameState.targetting = true;
		GameState.targettingCard = this.gameObject;
		Debug.Log("Waiting for target");
		while(!selected){
			if(target != null){
				selected = true;
				GameState.targetting = false;
				GameState.targettingCard = null;
				Debug.Log("Target selected");
				OnTargetSelect(target);
			}
			yield return null;
		}
	}

	public void Update(){

		if(GameState.targetting && GameState.targettingCard == this.gameObject){

			if(Input.GetMouseButton(0)){

				Debug.Log("Click registered");

				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

				if(hit.collider != null){
					Debug.Log("We hit something");
					if(ValidateTarget(hit.transform.gameObject)){
						target = hit.transform.gameObject;
					}
				}

			}

		}

	}

	public void Kill(){

		player.GetComponent<PlayerField>().discard.GetComponent<DiscardPile>().Discard(this.gameObject);
		OnKillAbility();

	}

}
