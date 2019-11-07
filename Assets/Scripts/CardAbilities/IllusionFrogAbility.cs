using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionFrogAbility : CardAbility
{

  /*
  *	Ability that triggers on death.
  *	Gives the player the option to bounce the card when it is killed by combat
  */
  public override void OnKillByCombatAbility(){
    Debug.Log("We should bounce");
    Bounce();
  }

}
