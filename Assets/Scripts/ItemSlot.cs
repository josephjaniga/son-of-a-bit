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
	public GameObject parent;
	public Inventory inventory;
	public Equipment equipment;

	public GameObject owner;

	public bool inventoryParent = false;
	public bool equipmentParent = false;

	public bool initialized = false;

	// Use this for initialization
	void Start () {
	
		initialize ();

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
		}

		gameObject.GetComponent<Image>().color = Color.white;

	}
	
	public void OnPointerClick(PointerEventData data){

		// LEFT CLICK on an ItemSlot
		if ( data.button == PointerEventData.InputButton.Left ) {
			if ( item != null ){
				if ( inventory.itemInHand != null ){
					Item swap = inventory.itemInHand;
					inventory.itemInHand = item;
					item = swap;
				} else {
					inventory.itemInHand = item;
					item = null;
				}
			} else if ( item == null && inventory.itemInHand != null ) {
				item = inventory.itemInHand;
				inventory.itemInHand = null;
			}
		}

		// RIGHT CLICK on an ItemSlot
		if ( data.button == PointerEventData.InputButton.Right ) {
			if ( item != null ){
				item = null;
			}
		}

		if ( !initialized && owner == null ){
			initialize();
		}

	}


	public void initialize(){

		parent = gameObject.transform.parent.gameObject;
		inventory = GameObject.Find("NewInventory").GetComponent<Inventory>();

		if ( owner != null )
			equipment = owner.GetComponent<Equipment>();
		
		if ( inventory != null ){
			inventoryParent = true;
		} else {
			inventory = GameObject.Find("NewInventory").GetComponent<Inventory>();
		}
		
		if ( equipment != null ){
			equipmentParent = true;
		}

	}



}
