using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{

    private GameObject deck;

    // Start is called before the first frame update
    void Start()
    {
        deck = Transform.Find("DeckView");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
