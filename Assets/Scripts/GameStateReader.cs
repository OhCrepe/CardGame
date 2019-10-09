using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateReader : MonoBehaviour {

	public Text displayMessage; // The text that provides info to the players
	public String activePlayer = "Turn player"; // Name of the active player

	// Update is called once per frame
	void Update () {

		if(GameState.targetting){
				displayMessage.text = activePlayer + " is deciding which card to target for the effect of "
						+ GameState.targettingCard.transform.Find("Name").GetComponent<Text>().text;
				return;
		}
		displayMessage.text = activePlayer + " is currently deciding what to do...";

	}

}
