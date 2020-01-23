using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility : MonoBehaviour {

	public GameObject player; // The player this card belongs to
	public int fieldTriggerCost; //The cost to trigger the field effect of this card
	public bool hasTriggerAbility, //Whether this card has an ability that can be triggered on the field
							hasStartOfTurnAbility,
							hasEndOfTurnAbility;

	public void Awake(){
		player = GameObject.Find("PlayerField");
	}

	public virtual void Attack(){

		StartCoroutine(WaitForAttackTarget());

	}

	private GameObject target = null;

	public virtual void OnKillAbility(bool combat){
		if(combat){
			OnKillByCombatAbility();
		}else{
			OnKillByEffectAbility();
		}
	}

	/*
	*	Check whether the activation of the card is legal
	*/
	public virtual bool ValidActivation(){
		PlayerField playerField = this.player.GetComponent<PlayerField>();
		if(playerField.gold > GetComponent<CardData>().cost){
			return ActivationRequirementsMet();
		}
		return false;
	}

	protected virtual bool ActivationRequirementsMet(){
		return true;
	}

	public virtual void OnKillByCombatAbility(){
		//DO NOTHING
	}
	public virtual void OnKillByEffectAbility(){
		//DO NOTHING
	}

	// What to do when a target is selected for this cards ability
	public virtual void OnTargetSelect(GameObject card){
		//DO NOTHING
	}

	//Validate that the target for this ability is valid
	public virtual bool ValidateTarget(GameObject card){
		return true;
	}

	// Ability that triggers when hired
	public virtual void OnHire(){
		CheckUtility();
	}

	// Ability that triggers on the field when clicked
	public virtual void OnFieldTrigger(){
		//DO NOTHING
	}

	// Ability that triggers on the field when clicked
	public virtual void StartOfTurnAbility(){
		//DO NOTHING
	}

	// Ability that triggers on the field when clicked
	public virtual void EndOfTurnAbility(){
		//DO NOTHING
	}

	public void CheckUtility(){
		if(this.transform.GetComponent<CardData>().cardType == CardData.Type.UTILITY){
			Kill(false);
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
	public IEnumerator WaitForAttackTarget(){

		bool selected = false;
		GameState.targetting = true;
		GameState.attacking= true;
		GameState.targettingCard = this.gameObject;
		Debug.Log("Waiting for target");
		while(!selected){
			if(target != null){
				selected = true;
				GameState.targetting = false;
				GameState.attacking = true;
				GameState.targettingCard = null;
				Debug.Log("Target selected");
				if(ValidAttackTarget(target)){
					OnAttackTargetSelect(target);
				}
			}
			yield return null;
		}
	}

	protected virtual bool ValidAttackTarget(GameObject target){

		if(target.transform.parent.GetComponent<Dropzone>().zoneType == Dropzone.Zone.FIELD){
			return true;
		}
		return false;

	}

	public void OnAttackTargetSelect(GameObject target){

		GetComponent<CardData>().DealDamageTo(target, true);
		target.GetComponent<CardData>().DealDamageTo(gameObject, true);

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

	public void Kill(bool combat){

		player.GetComponent<PlayerField>().discard.GetComponent<DiscardPile>().Discard(this.gameObject);
		OnKillAbility(combat);

	}

	public void Bounce(){
		Transform hand = player.GetComponent<PlayerField>().hand.transform;
		GetComponent<Draggable>().parentToReturnTo = hand;
		this.gameObject.transform.SetParent(hand);
		gameObject.SetActive(true);
		GetComponent<CardData>().Restore();
	}

}
