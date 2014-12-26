using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemSlot : MonoBehaviour, IPointerClickHandler {

	public enum SlotTypes{
		Generic
	};

	public int itemSlotType = (int)SlotTypes.Generic;
	public string itemSlotName = "Default";
	public Item item = null;
	public GameObject inventory;

	// Use this for initialization
	void Start () {

		inventory = gameObject.transform.parent.gameObject;

	}
	
	// Update is called once per frame
	void Update () {
	
		if ( item != null ){

			if ( item.itemIcon ){
				gameObject.GetComponent<Image>().sprite = item.itemIcon;
			} else {
				Debug.LogError("Sprite not found ", item.itemIcon );
			}


		} else {
			gameObject.GetComponent<Image>().sprite = null;
			gameObject.GetComponent<Image>().color = Color.gray;
		}

	}
	
	public void OnPointerClick(PointerEventData data){
		if ( item != null ){
			Debug.Log(item.name + " picked up");
			inventory.GetComponent<Inventory>().itemInHand = item;
			item = null;
		}
	}

}
