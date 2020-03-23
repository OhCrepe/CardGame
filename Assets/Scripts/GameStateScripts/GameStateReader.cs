using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateReader : MonoBehaviour {

	public Text displayMessage; // The text that provides info to the players
	public String activePlayer = "Turn player"; // Name of the active player

	// Update is called once per frame
	void LetsNotUpdate () {

		if(GameState.currentPhase == GameState.Phase.START){
			displayMessage.text = "It is the start of " + activePlayer + "'s turn";
			return;
		}

		if(GameState.currentPhase == GameState.Phase.DEBT){
			displayMessage.text = "Checking for " + activePlayer + "'s debt";
			return;
		}

		if(GameState.currentPhase == GameState.Phase.GOLD){
			displayMessage.text = "Giving " + activePlayer + "5 gold";
			return;
		}

		if(GameState.currentPhase == GameState.Phase.DRAW){
			displayMessage.text = activePlayer + " get's to draw a card";
			return;
		}

		if(GameState.currentPhase == GameState.Phase.START_EFFECTS){
			displayMessage.text = "Resolving start of turn effects";
			return;
		}

		if(GameState.currentPhase == GameState.Phase.END_EFFECTS){
			displayMessage.text = "Resolving end of turn effects";
			return;
		}

		if(GameState.currentPhase == GameState.Phase.END){
			displayMessage.text = "Ending " + activePlayer + "'s turn";
			return;
		}

		if(GameState.deciding){
			displayMessage.text = activePlayer + " is deciding whether to use the effect of "
				+ GameState.decidingCard.transform.Find("Name").GetComponent<Text>().text;
			return;
		}

		if(GameState.targetting){
			if(GameState.attacking){
				displayMessage.text = activePlayer + " is deciding which card to target for the attack of "
					+ GameState.targettingCard.transform.Find("Name").GetComponent<Text>().text;
				return;
			}
			displayMessage.text = activePlayer + " is deciding which card to target for the effect of "
				+ GameState.targettingCard.transform.Find("Name").GetComponent<Text>().text;
			return;
		}
		displayMessage.text = activePlayer + " is currently deciding what to do...";

	}

}
