using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class InventoryPanel : MonoBehaviour {

	public GameObject player;
	public Bit playerBit;

	public GameObject item;

	void Awake(){
		player = GameObject.Find("Player");
		if ( player != null){
			playerBit = player.GetComponent<Bit>();
		}
	}

	// Use this for initialization
	void Start () {
	
		player = GameObject.Find("Player");
		if ( player != null){
			playerBit = player.GetComponent<Bit>();
		}

		redrawInventory();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void redrawInventory(){

		// clean out the inventory panel game objects
		GameObject ip = GameObject.Find("InventoryPanel");
		while( ip.transform.childCount > 0 ){
			GameObject.Destroy(ip.transform.GetChild(0));
		}

//		for ( int i = 0; i < playerBit.inventory.equippedItems.Count; i++ ) {
//			// for every item in the equipment array thats not equipped move it to the inventory
//			if ( !playerBit.inventory.equippedItems[i].isEquipped ){
//				playerBit.inventory.itemInventory.Add(playerBit.inventory.equippedItems[i]);
//				playerBit.inventory.equippedItems.RemoveAt(i);
//				i = 0;
//			}
//		}
//
//		for ( int i = 0; i < playerBit.inventory.itemInventory.Count; i++ ) {
//			// for every item in the items inventory array thats equipped move it to the equipment
//			if ( playerBit.inventory.itemInventory[i].isEquipped ){
//				playerBit.inventory.equippedItems.Add(playerBit.inventory.itemInventory[i]);
//				playerBit.inventory.itemInventory.RemoveAt(i);
//				i = 0;
//			}
//		}	     



		// populate the equipped items gui
		for ( int i = 0; i < playerBit.inventory.equippedItems.Count; i++ ) {
			GameObject temp = Instantiate(item, Vector3.zero, Quaternion.identity) as GameObject;
			temp.GetComponent<Item>().cloneItem ( playerBit.inventory.equippedItems[i] );
			temp.name = playerBit.inventory.equippedItems[i].itemName;
			temp.transform.FindChild("Text").GetComponent<Text>().text = playerBit.inventory.equippedItems[i].itemName;

			ColorBlock cb = temp.GetComponent<Button>().colors;
			cb.normalColor = new Color(0f, 1f, 0.3f, 0.3f);
			cb.highlightedColor = new Color(0f, 1f, 0.3f, 0.7f);
			cb.pressedColor = new Color(0f, 1f, 0.3f, 1f);

			temp.GetComponent<Button>().colors = cb;
			temp.transform.SetParent(GameObject.Find("InventoryPanel").transform);
			//temp.GetComponent<Button>().onClick.AddListener(() => playerBit.inventory.toggleEquipped(temp.GetComponent<Item>()));
		}
		
		// populate the inventory gui
		for ( int i = 0; i < playerBit.inventory.itemInventory.Count; i++ ) {
			GameObject temp = Instantiate(item, Vector3.zero, Quaternion.identity) as GameObject;
			temp.GetComponent<Item>().cloneItem ( playerBit.inventory.itemInventory[i] );
			temp.name = playerBit.inventory.itemInventory[i].itemName;
			temp.transform.FindChild("Text").GetComponent<Text>().text = playerBit.inventory.itemInventory[i].itemName;

			ColorBlock cb = temp.GetComponent<Button>().colors;
			cb.normalColor = new Color(1f, 1f, 1f, 0.3f);
			cb.highlightedColor = new Color(1f, 1f, 1f, 0.7f);
			cb.pressedColor = new Color(1f, 1f, 1f, 1f);
			
			temp.GetComponent<Button>().colors = cb;
			temp.transform.SetParent(GameObject.Find("InventoryPanel").transform);
			//temp.GetComponent<Button>().onClick.AddListener(() => playerBit.inventory.toggleEquipped(temp.GetComponent<Item>()));
		}

	}

}
