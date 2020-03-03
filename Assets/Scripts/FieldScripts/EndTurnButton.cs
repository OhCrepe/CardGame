using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{

    /*
    *   Request that we end our turn to the server
    */
    public void OnButton()
    {
        if(!GameState.targetting){
            GameObject.Find("NetworkManager").GetComponent<NetworkConnection>().SendMessage("ENDTURN");
        }
    }

}
