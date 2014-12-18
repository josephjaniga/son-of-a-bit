using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public bool displayCharacter = false;
	public bool displayInventory = false;

	public GameObject characterPanel;
	public GameObject inventoryPanel;
	public GameObject p;

	// Use this for initialization
	void Start () {
		inventoryPanel = GameObject.Find("InventoryPanel");
		characterPanel = GameObject.Find("CharacterPanel");
		p = GameObject.Find("Player");

	}
	
	// Update is called once per frame
	void Update () {

		// hide all Panels
		if ( Input.GetKeyDown(KeyCode.Escape) ){
			displayCharacter = false;
			displayInventory = false;
		}

		// keypress c - toggle Character Panel
		if ( Input.GetKeyDown("c") ){
			displayCharacter = !displayCharacter;
		}

		// keypress i - toggle Inventory Panel
		if ( Input.GetKeyDown("i") ){
			displayInventory = !displayInventory;
		}

		if ( characterPanel != null ) {
			if ( displayCharacter ){
				characterPanel.SetActive(true);
			} else if ( !displayCharacter ){
				characterPanel.SetActive(false);
			}
		}

		if ( inventoryPanel != null ) {
			if ( displayInventory ){
				inventoryPanel.SetActive(true);
			} else if ( !displayInventory ){
				inventoryPanel.SetActive(false);
			}
		}

		// DEVTOOLS
		// devTools();
	}


	private void devTools(){

		GameObject player = GameObject.Find ("Player");

		// keypress j - spawn an item
		if ( Input.GetKeyDown("j") ){
			GameObject go = p.GetComponent<Inventory>().createRandomItem();
			p.GetComponent<Inventory>().addItemToInventory(go.GetComponent<Item>());
		}

		if ( Input.GetKeyDown(KeyCode.Alpha1) ){
			player.GetComponent<Weapon>().bullet = (GameObject)Resources.Load("Projectiles/FF_Bullet"); 
		}
		
		if ( Input.GetKeyDown(KeyCode.Alpha2) ){
			player.GetComponent<Weapon>().bullet = (GameObject)Resources.Load("Projectiles/FF_Rocket"); 
		}


	}
}
