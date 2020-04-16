using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckSelect : MonoBehaviour
{

    public GameObject localText;
    private Dropdown dropdown;

    void Awake(){
        dropdown = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        InitializeDeckSelect();
        GameState.localServer = false;

    }

    void Update(){
        if (Input.GetKeyUp(KeyCode.L))
        {
            GameState.localServer = !GameState.localServer;
            localText.SetActive(GameState.localServer);
        }
    }

    private void InitializeDeckSelect(){
        List<string> decks = DeckList.GetAllDeckNames();
        dropdown.AddOptions(decks);
    }

    /*
    *   Return to the main menu
    */
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /*
    *   Read the deck select, load a game
    */
    public void LoadGame(){
        GameState.selectedDeck = dropdown.options[dropdown.value].text;
        SceneManager.LoadScene("Test Scene");
    }


}
