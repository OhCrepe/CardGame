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

        Dropdown typeDropdown = GameObject.Find("CardTypeDropdown").GetComponent<Dropdown>();
        string type = typeDropdown.options[typeDropdown.value].text;

        Dropdown costDropdown = GameObject.Find("CardCostDropdown").GetComponent<Dropdown>();
        string cost = costDropdown.options[costDropdown.value].text;

        bool typeSpecified;
        bool costSpecified;

        CardData.Type cardType = CardData.Type.LORD;
        int costInt = 0;

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

        switch(cost.ToUpper()){

            case "ANY":
                costSpecified = false;
                break;

            case "9+":
                costSpecified = true;
                costInt = 9;
                break;

            default:
                costSpecified = true;
                costInt = int.Parse(cost);
                break;


        }

        foreach(Transform child in cardView.transform){

            GameObject card = child.gameObject;
            string name = child.Find("Name").gameObject.GetComponent<Text>().text.ToUpper();
            bool nameSearched = name.Contains(search);

            bool correctType = (!typeSpecified || cardType == card.GetComponent<CardData>().cardType);
            bool correctCost = false;
            if(costInt == 9){
                if(card.GetComponent<CardData>().cost >= costInt && costSpecified) correctCost = true;
            }else{
                if(card.GetComponent<CardData>().cost == costInt && costSpecified) correctCost = true;
            }
            if(!costSpecified) correctCost = true;

            card.SetActive(nameSearched && correctType && correctCost);

        }

    }

}
