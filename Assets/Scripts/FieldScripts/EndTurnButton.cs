using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{

    public void OnButton()
    {
        if(!GameState.targetting){
            GameObject.Find("NetworkManager").GetComponent<NetworkConnection>().SendMessage("ENDTURN");
        }
    }

}
