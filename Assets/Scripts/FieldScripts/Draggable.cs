using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class Draggable :  MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler{

	public static bool dragging = false; // Whether we are currently dragging a card

	private Vector2 startPosition; // Position of the mouse relative to the card it's dragging
	public Transform parentToReturnTo; // Parent that the card needs to go back to on end drag
	public float hoverScale; // How much bigger to make the card when we're mousing over and dragging it
	public GameObject player; // Player this card belongs to
	public GameObject abilityButton; // The button that we press to trigger this cards ability
	private Dropzone.Zone originalParent; // The parent of the card when we first started dragging

	public Transform placeholderParent = null; // Parent of the placeholder slot for the card when dragging
	GameObject placeholder=null; // Placeholder slot to indicate where our card will be dropped

	/*
	*	Play this card onto the field - but only if cost can be paid
	*/
	public void CallCard(){

		int cost = this.gameObject.GetComponent<CardData>().cost;
		if(player.GetComponent<PlayerField>().PayGold(cost)){
			parentToReturnTo = player.GetComponent<PlayerField>().field.transform;
			this.gameObject.GetComponent<CardAbility>().OnHire();
		}else{
			parentToReturnTo = player.GetComponent<PlayerField>().hand.transform;
		}

	}

	/*
	* Detects when the mouse hovers over the card
	*/
	public void OnPointerEnter(PointerEventData eventData){

		if(!dragging){
			//Increase the size of the card and render it in front
			this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(hoverScale, hoverScale, hoverScale);
			this.gameObject.GetComponent<Canvas>().sortingOrder = 10; //Layer of cards being held

			if(this.transform.parent.GetComponent<Dropzone>().zoneType == Dropzone.Zone.FIELD){
				ConfigureAbilityButton(true);
			}

		}

	}

	/*
	* Detects when the mouse stops hovering over the card
	*/
	public void OnPointerExit(PointerEventData eventData){

		//Decrease the size of the card back to normal, and render it alongside the other cards
		this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
		this.gameObject.GetComponent<Canvas>().sortingOrder = 9; //Layer of cards not being interacted with
		ConfigureAbilityButton(false);

	}

	/*
	* Check whether an ability button exists (defined by whether this is a minion or a utility), and
	* set it's activeness as to which of the two it is.
	*/
	private void ConfigureAbilityButton(bool active){
		if(this.gameObject.GetComponent<CardData>().cardType == CardData.Type.UTILITY){
			return;
		}
		if(!this.gameObject.GetComponent<CardAbility>().hasTriggerAbility){
			return;
		}
		abilityButton.SetActive(active);
	}

	/*
	* While the mouse is being held over the card, move it with the mouse
	*/
	public void OnBeginDrag(PointerEventData eventData){

		if(GameState.targetting)return;

		originalParent = this.transform.parent.GetComponent<Dropzone>().zoneType;

		if(originalParent == Dropzone.Zone.FIELD){
			return;
		}

		Debug.Log("OnBeginDrag");

		CreatePlaceholder();

		// Offset for smoother dragging
		Vector2 pos = this.transform.position;
		Vector3 cam = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0));
		this.startPosition = pos - new Vector2(cam.x, cam.y);

		// Sets locks to current parent
		parentToReturnTo = this.transform.parent;
		placeholderParent = parentToReturnTo;
		this.transform.SetParent(this.transform.root);
		this.transform.SetAsLastSibling();

		// Allow raycasting to pass through while being dragged
		GetComponent<CanvasGroup>().blocksRaycasts = false;
		dragging = true;

	}

	/*
	* Introduce a placeholder to indicate the space that a dragged card will fill in
	*/
	private void CreatePlaceholder(){

		placeholder = new GameObject();
		placeholder.transform.SetParent(this.transform.parent);
		LayoutElement le = placeholder.AddComponent<LayoutElement>();
		le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
		le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
		le.flexibleWidth = 0;
		le.flexibleHeight = 0;

		placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

	}

	/*
	*	Will make the card follow the mouse and also increase it's size and rendering priority
	*/
	public void OnDrag(PointerEventData eventData){

		if(GameState.targetting)return;

		if(originalParent == Dropzone.Zone.FIELD){
			return;
		}

		//Debug.Log("OnDrag");
		Vector3 cam = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0));
		this.transform.position = this.startPosition + new Vector2(cam.x, cam.y);
		this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(hoverScale, hoverScale, hoverScale);
		this.gameObject.GetComponent<Canvas>().sortingOrder = 11; //Layer of cards being dragged
		MovePlaceholder();

	}

	/*
	*	Update the position of the placeholder so that it is where the dragged card will be dropped
	*/
	private void MovePlaceholder(){

		if(placeholder.transform.parent != placeholderParent)
			placeholder.transform.SetParent(placeholderParent);

		int newSiblingIndex = placeholderParent.childCount;

		for (int i = 0; i < placeholderParent.childCount; i++){
			if(this.transform.position.x < placeholderParent.GetChild(i).position.x){

				newSiblingIndex = i;

				if(placeholder.transform.GetSiblingIndex() < newSiblingIndex)
					newSiblingIndex--;

				break;

			}
		}

		placeholder.transform.SetSiblingIndex(newSiblingIndex);

	}

	/*
	*	Drops the card where the placeholder is currently set
	*/
	public void OnEndDrag(PointerEventData eventData){

		if(GameState.targetting)return;

		if(originalParent == Dropzone.Zone.FIELD){
			return;
		}

		if(parentToReturnTo.GetComponent<Dropzone>().zoneType == Dropzone.Zone.FIELD){
			CallCard();
		}

		this.startPosition = new Vector2(0f, 0f);

		this.transform.SetParent(parentToReturnTo);
		this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

		this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

		GetComponent<CanvasGroup>().blocksRaycasts = true;

		dragging = false;
		Destroy(placeholder);

	}

}