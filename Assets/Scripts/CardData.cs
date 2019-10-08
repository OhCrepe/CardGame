using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour {

	/*
	* Stores information about this card such as cost
	*/

	public int cost; // Cost to hire this warrior/activate this utility
	public enum Type { MINION, UTILITY }; // Whether this card is a utility or a minion
	public Type cardType;
	
}
