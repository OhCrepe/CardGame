using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayMenu : MonoBehaviour
{

    void Awake(){
        GameObject.Find("GuideScrollbar").GetComponent<Scrollbar>().value = 1;
        StartCoroutine(WaitThenFixScrollbar());
    }

    /*
    *   Adjust the scrollbar after a short period of time
    */
    IEnumerator WaitThenFixScrollbar(){
        yield return new WaitForSeconds(0.01f);
        GameObject.Find("GuideScrollbar").GetComponent<Scrollbar>().value = 1;
    }

    public void Close(){
        Destroy(gameObject);
    }

}
