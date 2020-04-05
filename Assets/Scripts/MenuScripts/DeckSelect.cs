using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckSelect : MonoBehaviour
{

    private Dropdown dropdown;

    void Start(){
        dropdown = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        InitializeDeckSelect();

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
