using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartAttack : MonoBehaviour, IPointerClickHandler {

	private CardAbility ability; // The script that contains this cards ability

	/*
	* Triggers on start
	*/
	public void Start(){
		GetCardAbility();
	}

	/*
	* Works out this cards ability from the objects it is attached to
	*/
	public void GetCardAbility(){
		ability = transform.parent.gameObject.GetComponent<CardAbility>();
	}

	/*
	*	Triggers the trigger ability of a card when clicked
	*/
	public void OnPointerClick(PointerEventData eventData){
		ability.Attack();
	}

}
