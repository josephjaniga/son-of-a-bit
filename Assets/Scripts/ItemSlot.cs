using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemSlot : MonoBehaviour, IPointerClickHandler {

	public enum ItemType {
		Generic,
		Weapon,
		Armor,
		Accessory,
		Technology
	};

	public int itemSlotType = (int)ItemType.Generic;
	public string itemSlotName = "Default";
	public Item item = null;
	public GameObject parent;
	public Inventory inventory;
	public Equipment equipment;

	public Sprite empty;

	public GameObject owner;

	public bool inventoryParent = false;
	public bool equipmentParent = false;

	public bool initialized = false;

	// Use this for initialization
	void Start () {
	
		initialize ();

		empty = gameObject.GetComponent<Image>().sprite;
	}
	
	// Update is called once per frame
	void Update () {
	
		if ( item != null ){
			if ( item.itemIcon ){
				gameObject.GetComponent<Image>().color = Color.white;
				gameObject.GetComponent<Image>().sprite = item.itemIcon;
			} else {
				Debug.LogError("Sprite not found ", item.itemIcon );
			}
		} else {

			gameObject.GetComponent<Image>().sprite = empty;

			switch(itemSlotType){
			case (int)ItemType.Weapon:
				gameObject.GetComponent<Image>().color = Color.red;
				break;
			case (int)ItemType.Armor:
				gameObject.GetComponent<Image>().color = Color.blue;
				break;
			case (int)ItemType.Accessory:
				gameObject.GetComponent<Image>().color = Color.green;
				break;
			case (int)ItemType.Technology:
				gameObject.GetComponent<Image>().color = Color.yellow;
				break;
			case (int)ItemType.Generic:
			default:
				gameObject.GetComponent<Image>().color = Color.white;
				break;
			}

		}

	}
	
	public void OnPointerClick(PointerEventData data){

		// LEFT CLICK on an ItemSlot
		if ( data.button == PointerEventData.InputButton.Left ) {
			// if the slot isn't empty
			if ( item != null ){
				// and we have an item on the mouse
				if ( inventory.itemInHand != null ){
					Item swap = inventory.itemInHand;
					inventory.itemInHand = item;
					item = swap;
				} else {
					inventory.itemInHand = item;
					item = null;
				}
			// slots empty with item in hand
			} else if ( item == null && inventory.itemInHand != null ) {

				if ( itemSlotType == (int)ItemType.Generic || itemSlotType == inventory.itemInHand.type ){
					item = inventory.itemInHand;
					inventory.itemInHand = null;
				} else {

					GameObject sct = Resources.Load("UI/SCT") as GameObject;

					// clear all the alerts
					foreach (Transform child in GameObject.Find("Alerts").transform) {
						GameObject.Destroy(child.gameObject);
					}

					GameObject alert = Instantiate(sct, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
					alert.GetComponent<Text>().text = "That doesn't go there!";
					alert.transform.SetParent(GameObject.Find("Alerts").transform);
					alert.transform.localPosition = new Vector3(0f, -80f, 0f);
					alert.GetComponent<Text>().fontSize = 12;
					alert.GetComponent<SCT>().Timer = 5;
					alert.GetComponent<SCT>().Timeout = 5;

				}

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
