﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilderClickable : MonoBehaviour
{

    DeckBuilder deckBuilder;

    void Awake(){
        GameObject builder = GameObject.Find("DeckBuilder");
        if(builder != null){
            deckBuilder = builder.GetComponent<DeckBuilder>();
        }
    }

    /*
    *   Detect whether or not we're in the deck, and call the correct method accordingly.
    */
    void OnMouseDown(){

        if(transform.parent.gameObject.name == "CardSelection"){
            deckBuilder.AddToDeck(this.gameObject);
        }else if(transform.parent.gameObject.name == "DeckView"){
            deckBuilder.RemoveFromDeck(this.gameObject);
        }

    }

}