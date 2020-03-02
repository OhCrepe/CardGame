using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dropzone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public enum Zone { HAND, FIELD, SEARCH }; // Possible locations this dropzone could be
    public Zone zoneType = Zone.HAND; // Zone this dropzone is

    /*
    * Sets the zone that the dragged card (if any) will be dropped to, and changes the
    placeholder parent
    */
    public void OnPointerEnter(PointerEventData eventData){

        if(eventData.pointerDrag == null)
        return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null) {
            d.placeholderParent = this.transform;
        }

    }

    /*
    * Undoes the actions of OnPointerEnter if we haven't dropped the card by this point
    */
    public void OnPointerExit(PointerEventData eventData){

        if(eventData.pointerDrag == null)
        return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeholderParent == this.transform) {
            d.placeholderParent = d.parentToReturnTo;
        }

    }

    /*
    * Sets the zone of the dragged card to be that which the placeholder is currently in
    */
    public void OnDrop(PointerEventData eventData){

        if(GameState.currentPhase != GameState.Phase.MAIN) return;
        if(GameState.targetting)return;

        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null) {
            if(d.parentToReturnTo.GetComponent<Dropzone>().zoneType == Dropzone.Zone.HAND){
                d.parentToReturnTo = this.transform;
            }
        }

    }

}
