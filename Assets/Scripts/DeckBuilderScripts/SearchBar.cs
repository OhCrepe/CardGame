using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchBar : MonoBehaviour
{

    GameObject cardView;

    void Awake(){
        cardView = GameObject.Find("CardSelection");
    }

    /*
    *   Update the card view with the currently searched for cards
    */
    public void UpdateCardView(){

        //string search = transform.Find("Text").GetComponent<Text>().text.ToUpper();
        string search = GetComponent<InputField>().text.ToUpper();

        foreach(Transform child in cardView.transform){

            GameObject card = child.gameObject;
            string name = child.Find("Name").gameObject.GetComponent<Text>().text.ToUpper();

            Debug.Log(name + " ~ " + search);

            card.SetActive(name.Contains(search));

        }

    }

}
