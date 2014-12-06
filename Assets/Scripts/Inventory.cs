using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public int credits = 0;
	public Bit bit;

	public List<Item> itemInventory = new List<Item>();

	public List<Item> equippedItems = new List<Item>();

	// Use this for initialization
	void Start () {

		bit = gameObject.GetComponent<Bit>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void addCredits(int Amount){
		credits += Amount;
	}
	
}