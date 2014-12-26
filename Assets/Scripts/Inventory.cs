using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public GameObject genericItem;

	public GameObject ip;
	public List<Item> itemInventory = new List<Item>();
	public int inventorySize = 1;
	public List<ItemSlot> slots = new List<ItemSlot>();

	public int idCount = 0;
	
	private Transform itemsContainer;

	public int credits = 0;
	public Bit bit;

	public Item itemInHand = null;
	public GameObject itemOnHandIcon = null;

	// Use this for initialization
	void Start () {

		itemOnHandIcon = GameObject.Find("ItemOnHandIcon");
		itemOnHandIcon.SetActive(false);

		// find all the stuff
		GameObject temp = Resources.Load("UI/Slot") as GameObject;
		itemsContainer = GameObject.Find("Items").transform;
		int itemCounter = GameObject.Find("Items").transform.childCount;
		
		// make the slots
		for ( int i =0; i<inventorySize; i++ ){
			GameObject s = Instantiate(temp, Vector3.zero, Quaternion.identity) as GameObject;
			s.transform.SetParent(gameObject.transform);
			slots.Add(s.GetComponent<ItemSlot>());
		}

		// grab the items from the scene
		foreach ( Transform item in GameObject.Find("Items").transform ){
			Item i = item.GetComponent<Item>();
			if ( i != null ){
				idCount++;
				i.itemId = idCount;	
				itemInventory.Add(i);
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
		if ( itemInHand != null ){

			itemOnHandIcon.transform.SetParent(GameObject.Find("Canvas").transform);

			Vector3 temp = Input.mousePosition;
			temp.x += 18;
			temp.y -= 18;

			// set position
			itemOnHandIcon.transform.position = temp;

			// set image
			itemOnHandIcon.GetComponent<Image>().sprite = itemInHand.itemIcon;

			// show
			itemOnHandIcon.SetActive(true);


		} else {

			//hide
			itemOnHandIcon.SetActive(false);
		}

	}


	public void addItemToInventory(Item item){
		
		// follow the same process on start
		// item should be an INSTANCE in the ITEMS set
		// item instance should have a valid ID
		if ( item.itemId != 0 ){
			if ( countAvailableSlots() > 0 ){
				getFirstAvailableSlot().item = item;
			}
		}
		
	}

	public void addCredits(int Amount){
		credits += Amount;
	}
	
	public GameObject createRandomItem(){
		
		// make it
		GameObject go = Instantiate(genericItem, Vector3.zero, Quaternion.identity) as GameObject;
		go.GetComponent<Item>().randomize();
		go.transform.SetParent(itemsContainer);
		go.GetComponent<Item>().askForId();
		go.name = go.GetComponent<Item>().itemName;
		
		return go;
		
	}

	public int countAvailableSlots(){
		int count = 0;
		for ( int i=0; i<inventorySize; i++ ){
			if ( slots[i].GetComponent<ItemSlot>().item == null ){
				count++;
			}
		}
		Debug.Log (count + " available slots");
		return count;
	}

	public ItemSlot getFirstAvailableSlot(){
		ItemSlot iS = null;
		for ( int i=0; i<inventorySize; i++ ){
			if ( slots[i].GetComponent<ItemSlot>().item == null ){
				iS = slots[i].GetComponent<ItemSlot>();
				break;
			}
		}
		return iS;
	}



	
}
