using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{

    /*
    *   Return to the main menu
    */
    public void OnButton()
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkConnection>().Disconnect();
        SceneManager.LoadScene("MainMenu");
        GameState.gameOver = false;
    }

}
