using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMap : MonoBehaviour
{

    private static SortedDictionary<string, GameObject> cards;
    private const string prefabPath = "Prefab/Cards";

    /*
    *   Initialize the card "database"
    */
    static CardMap(){
        cards = new SortedDictionary<string, GameObject>();

        Object[] cardArray;
        cardArray = Resources.LoadAll(prefabPath, typeof(GameObject));
        foreach(GameObject cardObject in cardArray){
            GameObject card = (GameObject)cardObject;
            string cardName = card.transform.Find("Name").GetComponent<Text>().text;
            Debug.Log("Map - " + cardName);
            cards.Add(cardName, card);
        }

    }

    /*
    *   Create the card in the given zone of the field
    */
    public static GameObject InstantiateToZone(string cardName, Transform zone){
        return Instantiate(cards[cardName], zone);
    }

    /*
    *   Return the type of card that the given name is
    */
    public static string GetCardType(string cardName){
        Debug.Log(cardName);
        return cards[cardName].tag;
    }

}
