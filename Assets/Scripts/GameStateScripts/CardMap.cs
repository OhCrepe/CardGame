using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMap : MonoBehaviour
{

    private static SortedDictionary<string, GameObject> cards;
    private const string prefabPath = "Prefab/Cards";

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

/*
        Object[] lordArray;
        lordArray = Resources.LoadAll(prefabPath + "/Lord Card", typeof(GameObject));
        foreach(GameObject lordObject in lordArray){
            GameObject card = (GameObject)lordObject;
            string cardName = card.transform.Find("Name").GetComponent<Text>().text;
            cards.Add(cardName, card);
        }
*/
    }

    public static GameObject InstantiateToZone(string cardName, Transform zone){
        return Instantiate(cards[cardName], zone);
    }

    public static string GetCardType(string cardName){
        Debug.Log(cardName);
        return cards[cardName].tag;
    }

}
