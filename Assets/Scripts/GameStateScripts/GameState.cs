using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	public static bool targetting = false, attacking = false, deciding = false, dragging = false;
	public static GameObject targettingCard = null, decidingCard = null;

	public enum Phase {START, DEBT, GOLD, START_EFFECTS, DRAW, MAIN, END_EFFECTS, END};
	public static Phase currentPhase;

	public static GameObject currentPlayer;

	static GameState(){
		currentPlayer = GameObject.Find("PlayerField");
		MainPhase();
	}

	/*
	*	Logic that handles the start of a player's turn
	*/
	public static void StartPhase(){
		currentPhase = Phase.START;
		DebtPhase();
	}

	/*
	*	Logic that handles checking for a player's debt
	* Give them a debt counter if no gold left, or they lose the game if they have
	* no gold AND a debt counter
	*/
	public static void DebtPhase(){
		currentPhase = Phase.DEBT;
		GoldPhase();
	}

	/*
	*	Logic that handles the player's gold gained for the turn.
	* DEFUALT: Give 5 gold.
	*/
	public static void GoldPhase(){
		currentPhase = Phase.GOLD;
		currentPlayer.GetComponent<PlayerField>().GainGold(5);
		StartEffectsPhase();
	}

	/*
	* Resolve all effects that trigger at the beginning of a player's turn
	*/
	public static void StartEffectsPhase(){
		currentPhase = Phase.START_EFFECTS;
		GameObject[] lords = GameObject.FindGameObjectsWithTag("Lord");
		foreach(GameObject lord in lords){
			lord.GetComponent<CardAbility>().StartOfTurnAbility();
		}
		GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
		foreach(GameObject card in cards){
			card.GetComponent<CardAbility>().StartOfTurnAbility();
			card.GetComponent<CardAbility>().ResetOncePerTurn();
		}
		DrawPhase();
	}

	/*
	*	Turn player draws a card
	*/
	public static void DrawPhase(){
		currentPhase = Phase.DRAW;
		MainPhase();
	}

	/*
	*	In this phase the turn player gets to play their cards, hire units
	* and enter combat
	*/
	public static void MainPhase(){
		currentPhase = Phase.MAIN;
	}

	/*
	*	Resolve all effects that trigger at the end of a player's turn
	*/
	public static void EndEffectsPhase(){

		currentPhase = Phase.END_EFFECTS;
		GameObject[] lords = GameObject.FindGameObjectsWithTag("Lord");
		foreach(GameObject lord in lords){
			lord.GetComponent<CardAbility>().EndOfTurnAbility();
		}
		GameObject[] cards = GameObject.FindGameObjectsWithTag("Lord");
		foreach(GameObject card in cards){
			card.GetComponent<CardAbility>().EndOfTurnAbility();
		}
		EndPhase();

	}

	/*
	*	End the current player's turn, give control to their opponent
	*/
	public static void EndPhase(){
		if(currentPlayer == null){
			Debug.Log("Player null");
		}
		//currentPlayer.GetComponent<PlayerField>().PayWages();
		currentPhase = Phase.END;
		StartPhase();
	}

}
