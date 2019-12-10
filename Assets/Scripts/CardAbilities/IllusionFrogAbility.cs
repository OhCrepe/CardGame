using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class IllusionFrogAbility : CardAbility
{

  /*
  *	Ability that triggers on death.
  *	Gives the player the option to bounce the card when it is killed by combat
  */
  public override void OnKillByCombatAbility(){
    GameState.deciding = true;
    GameState.decidingCard = this.gameObject;
    //if(EditorUtility.DisplayDialog("Place Selection On Surface?", "Do you want to use the effect of Illusion Frog", "Yes", "No")){
      Bounce();
    //}else{
    //  player.GetComponent<PlayerField>().discard.GetComponent<DiscardPile>().Discard(this.gameObject);
    //}
    GameState.deciding = false;
    GameState.decidingCard = null;

  }

}
