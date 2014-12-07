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

		// keypress c - toggle Character Panel
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

	}
}
