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

        string search = GetComponent<InputField>().text.ToUpper();
        Dropdown typeDropdown = GameObject.Find("CardDropdown").GetComponent<Dropdown>();
        string type = typeDropdown.options[typeDropdown.value].text;
        bool typeSpecified;
        CardData.Type cardType = CardData.Type.LORD;

        switch(type.ToUpper()){

            case "LORDS":
                typeSpecified = true;
                cardType = CardData.Type.LORD;
                break;

            case "UNITS":
                typeSpecified = true;
                cardType = CardData.Type.MINION;
                break;

            case "UTILITIES":
                typeSpecified = true;
                cardType = CardData.Type.UTILITY;
                break;

            default:
                typeSpecified = false;
                break;

        }

        foreach(Transform child in cardView.transform){

            GameObject card = child.gameObject;
            string name = child.Find("Name").gameObject.GetComponent<Text>().text.ToUpper();
            bool nameSearched = name.Contains(search);

            bool correctType = (!typeSpecified || cardType == card.GetComponent<CardData>().cardType);

            card.SetActive(nameSearched && correctType);

        }

    }

}
