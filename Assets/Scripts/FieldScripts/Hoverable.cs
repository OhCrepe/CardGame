using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public float hoverScale; // How much bigger to make the card when we're mousing over and dragging it

    /*
    * Detects when the mouse hovers over the card
    */
    public void OnPointerEnter(PointerEventData eventData){

        if(!GameState.dragging){
            //Increase the size of the card and render it in front
            GetComponent<RectTransform>().localScale = new Vector3(hoverScale, hoverScale, hoverScale);
            if(transform.parent != null){
                if(transform.parent.GetComponent<Dropzone>() != null){
                    if(transform.parent.GetComponent<Dropzone>().zoneType == Dropzone.Zone.SEARCH){
                        GetComponent<Canvas>().sortingOrder = 15; //Layer of cards being held
                        return;
                    }
                }
            }
            GetComponent<Canvas>().sortingOrder = 11; //Layer of cards being held

        }

    }

    /*
    * Detects when the mouse stops hovering over the card
    */
    public void OnPointerExit(PointerEventData eventData){

        //Decrease the size of the card back to normal, and render it alongside the other cards
        GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        if(transform.parent != null){
            if(transform.parent.GetComponent<Dropzone>() != null){
                if(transform.parent.GetComponent<Dropzone>().zoneType == Dropzone.Zone.SEARCH){
                    GetComponent<Canvas>().sortingOrder = 15; //Layer of cards being held
                    return;
                }
            }
        }
        GetComponent<Canvas>().sortingOrder = 10; //Layer of cards being held

    }

    /*
    * Increase it's size and rendering priority
    */
    public void OnDrag(PointerEventData eventData){

        if(GameState.currentPhase != GameState.Phase.MAIN) return;
        if(GameState.targetting)return;

        GetComponent<RectTransform>().localScale = new Vector3(hoverScale, hoverScale, hoverScale);
        GetComponent<Canvas>().sortingOrder = 11; //Layer of cards being dragged

    }

}
