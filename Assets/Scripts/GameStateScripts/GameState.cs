using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	public static bool targetting = false, attacking = false, deciding = false;
	public static GameObject targettingCard = null, decidingCard = null;

	public enum Phase {START, DEBT, GOLD, START_EFFECTS, DRAW, MAIN, END_EFFECTS, END};
	public GameObject currentPlayer;
	public Phase currentPhase;

	void Start(){
		currentPhase = Phase.START;
	}

	/*
	*	Logic that handles the start of a player's turn
	*/
	private void StartPhase(){

	}

	/*
	*	Logic that handles checking for a player's debt
	* Give them a debt counter if no gold left, or they lose the game if they have
	* no gold AND a debt counter
	*/
	private void DebtPhase(){

	}

	/*
	*	Logic that handles the player's gold gained for the turn.
	* DEFUALT: Give 5 gold.
	*/
	private void GoldPhase(){

	}

	/*
	* Resolve all effects that trigger at the beginning of a player's turn
	*/
	private void StartEffectsPhase(){

	}

	/*
	*	Turn player draws a card
	*/
	private void DrawPhase(){

	}

	/*
	*	In this phase the turn player gets to play their cards, hire units
	* and enter combat
	*/
	private void MainPhase(){

	}

	/*
	*	Resolve all effects that trigger at the end of a player's turn
	*/
	private void EndEffectsPhase(){

	}

	/*
	*	End the current player's turn, give control to their opponent
	*/
	private void EndPhase(){

	}

}
