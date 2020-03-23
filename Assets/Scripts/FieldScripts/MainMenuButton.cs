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
        SceneManager.LoadScene("MainMenu");
    }

}
