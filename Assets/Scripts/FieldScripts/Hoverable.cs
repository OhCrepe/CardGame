﻿using System.Collections;
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

      this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(hoverScale, hoverScale, hoverScale);
      this.gameObject.GetComponent<Canvas>().sortingOrder = 10; //Layer of cards being held

  }

  /*
  * Detects when the mouse stops hovering over the card
  */
  public void OnPointerExit(PointerEventData eventData){

    //Decrease the size of the card back to normal, and render it alongside the other cards
    this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    this.gameObject.GetComponent<Canvas>().sortingOrder = 9; //Layer of cards not being interacted with

  }

}
