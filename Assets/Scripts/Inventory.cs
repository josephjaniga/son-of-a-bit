using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public InventoryPanel ip;
	public int credits = 0;
	public Bit bit;
	public int idCount = 0;
	
	public List<Item> itemInventory = new List<Item>();
	public List<Item> equippedItems = new List<Item>();
	

	// Use this for initialization
	void Start () {

		bit = gameObject.GetComponent<Bit>();

		for ( int i = 0; i < itemInventory.Count; i++ ) {
			idCount++;
			itemInventory[i].itemId = idCount;
		}

		for ( int i = 0; i < equippedItems.Count; i++ ) {
			idCount++;
			equippedItems[i].itemId = idCount;
			equippedItems[i].isEquipped = true;
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

		Debug.Log ("clicked - " + itemInstance.itemName);

		//itemInstance.isEquipped = !itemInstance.isEquipped;

		//ip.redrawInventory();

	}
	
}