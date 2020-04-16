using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void LoadGame(){
        SceneManager.LoadScene("DeckSelect");
    }

    public void LoadDeckBuilder(){
        SceneManager.LoadScene("DeckBuilder");
    }

    public void LoadHowToPlay(){
        SceneManager.LoadScene("HowToPlay", LoadSceneMode.Additive);
    }



}
