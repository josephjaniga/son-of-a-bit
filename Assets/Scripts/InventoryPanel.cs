using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class InventoryPanel : MonoBehaviour {

	public GameObject player;
	public Bit playerBit;

	public GameObject item;
	public Transform ip;

	void Awake(){
		player = GameObject.Find("Player");
		if ( player != null){
			playerBit = player.GetComponent<Bit>();
		}

		ip = GameObject.Find("InventoryPanel").transform;
	}

	// Use this for initialization
	void Start () {
	
		player = GameObject.Find("Player");
		if ( player != null){
			playerBit = player.GetComponent<Bit>();
		}

		recreateInventoryGui();

	}
	
	// Update is called once per frame
	void Update () {

		if ( playerBit.inventory.itemInventory.Count != ip.transform.childCount ){

			// if the inventory doesnt match the gui
			recreateInventoryGui();

		} else {

			for ( int i = 0; i < playerBit.inventory.itemInventory.Count; i++ ) {
				
				Item iI = playerBit.inventory.itemInventory[i];
				
				GameObject go = getItemGameObjectById(iI.itemId);
				
				if ( iI.isEquipped ){
					go.GetComponent<Image>().color = new Color(0f, 1f, 0.3f, 1f);
				} else {
					go.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
				}
				
			}

		}

	}

	public void buttonClick(Item temp){

		playerBit.inventory.toggleEquipped(temp);

	}


	public Button getItemButtonById(int id){

		Button button = null;

		foreach ( Transform child in ip ){
			Item temp = child.GetComponent<Item>();

			if ( temp.itemId == id ){
				button = child.GetComponent<Button>();
				break;
			}
		}

		return button;
	}

	public GameObject getItemGameObjectById(int id){
		
		GameObject go = null;
		
		foreach ( Transform child in ip ){
			Item temp = child.GetComponent<Container>().item;
			
			if ( temp.itemId == id ){
				go = child.gameObject;
				break;
			}
		}
		
		return go;
	}

	public void recreateInventoryGui(){

		Debug.Log (ip.name);
		foreach(Transform child in ip.transform) {
			Destroy(child.gameObject);
		}

//		while( ip.transform.childCount > 0 ){
//			GameObject.Destroy(ip.transform.GetChild(0));
//		}

		// populate the inventory gui
		for ( int i = 0; i < playerBit.inventory.itemInventory.Count; i++ ) {
			GameObject temp = Instantiate(item, Vector3.zero, Quaternion.identity) as GameObject;
			temp.GetComponent<Container>().item = playerBit.inventory.itemInventory[i];
			temp.name = playerBit.inventory.itemInventory[i].itemName + " - ID:" + playerBit.inventory.itemInventory[i].itemId;
			temp.transform.FindChild("Text").GetComponent<Text>().text = playerBit.inventory.itemInventory[i].itemName;
			
			ColorBlock cb = temp.GetComponent<Button>().colors;
			
			if ( playerBit.inventory.itemInventory[i].isEquipped ){
				//Equipped Item Colors
				cb.normalColor = new Color(0f, 1f, 0.3f, 0.5f);
				cb.highlightedColor = new Color(0f, 1f, 0.3f, 0.7f);
				cb.pressedColor = new Color(0f, 1f, 0.3f, 1f);
			} else {
				cb.normalColor = new Color(1f, 1f, 1f, 0.5f);
				cb.highlightedColor = new Color(1f, 1f, 1f, 0.7f);
				cb.pressedColor = new Color(1f, 1f, 1f, 1f);
			}
			
			Item tempItem = temp.GetComponent<Container>().item;
			temp.GetComponent<Button>().colors = cb;
			temp.transform.SetParent(ip.transform);
			temp.GetComponent<Button>().onClick.RemoveAllListeners();
			if ( tempItem != null && temp.GetComponent<Button>() != null ){
				temp.GetComponent<Button>().onClick.AddListener(() => buttonClick(tempItem));
			}
		}
	}




}
