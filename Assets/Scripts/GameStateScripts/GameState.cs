using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	public static bool targetting = false, attacking = false, deciding = false;
	public static GameObject targettingCard = null, decidingCard = null;

	public enum Phase {START, DEBT, GOLD, START_EFFECTS, DRAW, MAIN, END_EFFECTS, END};
	public static GameObject currentPlayer;
	public static Phase currentPhase;

	static GameState(){
		currentPlayer = GameObject.Find("PlayerField");
		StartPhase();
	}

	/*
	*	Logic that handles the start of a player's turn
	*/
	private static void StartPhase(){
		currentPhase = Phase.START;
		DebtPhase();
	}

	/*
	*	Logic that handles checking for a player's debt
	* Give them a debt counter if no gold left, or they lose the game if they have
	* no gold AND a debt counter
	*/
	private static void DebtPhase(){
		currentPhase = Phase.DEBT;
		GoldPhase();
	}

	/*
	*	Logic that handles the player's gold gained for the turn.
	* DEFUALT: Give 5 gold.
	*/
	private static void GoldPhase(){
		currentPhase = Phase.GOLD;
		currentPlayer.GetComponent<PlayerField>().GainGold(5);
		StartEffectsPhase();
	}

	/*
	* Resolve all effects that trigger at the beginning of a player's turn
	*/
	private static void StartEffectsPhase(){
		currentPhase = Phase.START_EFFECTS;
		DrawPhase();
	}

	/*
	*	Turn player draws a card
	*/
	private static void DrawPhase(){
		currentPhase = Phase.DRAW;
		currentPlayer.GetComponent<DeckInteraction>().DrawCard();
		MainPhase();
	}

	/*
	*	In this phase the turn player gets to play their cards, hire units
	* and enter combat
	*/
	private static void MainPhase(){
		currentPhase = Phase.MAIN;

	}

	/*
	*	Resolve all effects that trigger at the end of a player's turn
	*/
	private static void EndEffectsPhase(){
		currentPhase = Phase.END_EFFECTS;
		EndPhase();
	}

	/*
	*	End the current player's turn, give control to their opponent
	*/
	private static void EndPhase(){
		currentPhase = Phase.END;
		StartPhase();
	}

}
