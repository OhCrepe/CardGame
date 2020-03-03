using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility : MonoBehaviour {

	public GameObject player; // The player this card belongs to
	public int fieldTriggerCost; //The cost to trigger the field effect of this card
	public bool hasTriggerAbility, //Whether this card has an ability that can be triggered on the field
							hasStartOfTurnAbility,
							hasEndOfTurnAbility;
	public bool isOncePerTurn;

	public bool oncePerTurnUsed;
	private GameObject target = null;

	/*
	*	Locate the player object
	*/
	public void Awake(){
		player = GameObject.Find("PlayerField");
	}

	/*
	*	Beging waiting for an attack target
	*/
	public virtual void Attack(){

		StartCoroutine(WaitForAttackTarget());

	}

	//Validate that the target for this ability is valid
	public virtual bool ValidateTarget(GameObject card){
		return true;
	}

	/*
	*	Wait for an attack target to be chosen by the player
	*/
	public IEnumerator WaitForAttackTarget(){

		bool selected = false;
		GameState.targetting = true;
		GameState.attacking = true;
		GameState.targettingCard = this.gameObject;
		Debug.Log("Waiting for target");
		while(!selected){
			if(target != null){
				selected = true;
				GameState.targetting = false;
				GameState.attacking = false;
				GameState.targettingCard = null;
				Debug.Log("Target selected");
				if(ValidAttackTarget(target)){
					OnAttackTargetSelect(target);
					target = null;
				}
			}
			yield return null;
		}
	}

	/*
	*	Validate that the chosen attack target is valid
	*/
	protected virtual bool ValidAttackTarget(GameObject target){

		if(target.transform.parent.GetComponent<Dropzone>().zoneType == Dropzone.Zone.FIELD){
			return true;
		}
		return false;

	}

	/*
	*	Send the message to the server to indicate that we'd like to attack
	*/
	public void OnAttackTargetSelect(GameObject target){

		string message = "ATTACK#" + GetComponent<CardData>().GetId() + "#" + target.GetComponent<CardData>().GetId();
		GameObject.Find("NetworkManager").GetComponent<NetworkConnection>().SendMessage(message);

	}

	/*
	*	Checks for player input in the form of clicking on cards
	*/
	public void Update(){

		if(GameState.targetting && GameState.targettingCard == this.gameObject){

			if(Input.GetMouseButton(0)){

				Debug.Log("Click registered");

				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

				if(hit.collider != null){
					Debug.Log("We hit something");
					if ((!GameState.attacking && ValidateTarget(hit.transform.gameObject)) || (GameState.attacking && ValidAttackTarget(hit.transform.gameObject))){
						target = hit.transform.gameObject;
						string message = "TARGET#" + target.GetComponent<CardData>().GetId();
						GameObject.Find("NetworkManager").GetComponent<NetworkConnection>().SendMessage(message);
					}
				}

			}



		}

	}

	/*
	*	Kill this card - send it to the discard pile
	*/
	public void Kill(bool combat){

		player.GetComponent<PlayerField>().discard.GetComponent<DiscardPile>().Discard(this.gameObject);

	}

	/*
	*	Bounce the unit - return it to the hand
	*/
	public void Bounce(){
		Transform hand = player.GetComponent<PlayerField>().hand.transform;
		GetComponent<Draggable>().parentToReturnTo = hand;
		this.gameObject.transform.SetParent(hand);
		gameObject.SetActive(true);
	}

}
