using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour {

	public GameObject playerObject, cardObject;

	// Use this for initialization
	void Start () {


		if(!isLocalPlayer){
			Debug.Log("Someone else has connected");
			return;
		}

		CmdSpawnPlayer();
		Debug.Log("We have connected");

	}

	/*
	*	Create the player object
	*/
	[Command]
	void CmdSpawnPlayer(){

		GameObject player = Instantiate(playerObject);
		NetworkServer.SpawnWithClientAuthority(player, connectionToClient);

		CmdSpawnCard(cardObject, player);
		CmdSpawnCard(cardObject, player);
		CmdSpawnCard(cardObject, player);
		CmdSpawnCard(cardObject, player);
		CmdSpawnCard(cardObject, player);
		CmdSpawnCard(cardObject, player);
		CmdSpawnCard(cardObject, player);


	}

	[Command]
	void CmdSpawnCard(GameObject prefab, GameObject player){
		GameObject card = Instantiate(prefab);
		NetworkServer.SpawnWithClientAuthority(card, connectionToClient);
		player.GetComponent<DeckInteraction>().AddToTop(card);
		card.GetComponent<Draggable>().player = player;
		card.GetComponent<CardAbility>().player = player;
		//RpcConfigureCard(card, player);
		Transform deckObject = player.GetComponent<PlayerField>().deck.transform;
		card.transform.SetParent(deckObject);
	}

	[ClientRpc]
	void RpcConfigureCard(GameObject card, GameObject player){
		Transform deckObject = player.GetComponent<PlayerField>().deck.transform;
		card.transform.SetParent(deckObject);
	}

}
