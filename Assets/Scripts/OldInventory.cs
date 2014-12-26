using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OldInventory : MonoBehaviour {

	public GameObject genericItem;

	public InventoryPanel ip;
	public int credits = 0;
	public Bit bit;
	public int idCount = 0;
	
	public List<Item> itemInventory = new List<Item>();

	public Transform itemsContainer;
	
	// Use this for initializationss
	void Start () {

		bit = gameObject.GetComponent<Bit>();

		int itemCounter = GameObject.Find("Items").transform.childCount;

		itemsContainer = GameObject.Find("Items").transform;

		/*
		for ( int i = 0; i < itemCounter; i++ ) {
			idCount++;
			itemInventory[i].itemId = idCount;
		}
		*/

		foreach ( Transform item in GameObject.Find("Items").transform ){
			Item i = item.GetComponent<Item>();
			if ( i != null ){
				idCount++;
				i.itemId = idCount;	
				if ( i.isEquipped ){
					itemInventory.Add(i);
				}
			}
		}

		ip = GameObject.Find("FatherBit").GetComponent<InventoryPanel>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void addCredits(int Amount){
		credits += Amount;
	}

	public void toggleEquipped(Item itemInstance){
		itemInstance.isEquipped = !itemInstance.isEquipped;
	}

	public void addItemToInventory(Item item){

		// follow the same process on start
		// item should be an INSTANCE in the ITEMS set
		// item instance should have a valid ID
		if ( item.itemId != 0){
			
			item.isEquipped = true;

			// add the item to inventory items array
			itemInventory.Add(item);
			
			// redraw the gui elements
			ip.recreateInventoryGui();

		}

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
	
}