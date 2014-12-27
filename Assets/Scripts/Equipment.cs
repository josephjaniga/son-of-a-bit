using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Equipment : MonoBehaviour {

	public bool isPlayer = false;
	public bool isVehicle = false;

	public int weaponSlots;
	public int armorSlots;
	public int accessorySlots;
	public int techSlots;
	public int genericSlots;

	public GameObject equipPanel;
	public GameObject itemContainer;

	public List<ItemSlot> genericSlotList;

	// Use this for initialization
	void Awake () {

		//make an Equipment panel
		GameObject canvas = GameObject.Find("Canvas");
		GameObject temp = Resources.Load("UI/Slot") as GameObject;
		GameObject panel = Resources.Load("UI/EquipmentPanel") as GameObject;
		equipPanel = Instantiate(panel, Vector3.zero, Quaternion.identity) as GameObject;
		equipPanel.transform.SetParent(GameObject.Find ("EquipmentPanels").transform);
		equipPanel.transform.localPosition = Vector3.zero;
		equipPanel.name = gameObject.name + "EquipmentPanel";

		equipPanel.transform.Find("Name").GetComponent<Text>().text = gameObject.name;
		itemContainer = equipPanel.transform.Find("Container").gameObject;

		// setup the slots
		genericSlotList = new List<ItemSlot>();

		for ( int i=0; i<weaponSlots; i++ ){
			GameObject s = Instantiate(temp, Vector3.zero, Quaternion.identity) as GameObject;
			s.GetComponent<ItemSlot>().itemSlotType = (int)ItemSlot.ItemType.Weapon;
			s.transform.SetParent(itemContainer.transform);
			s.GetComponent<ItemSlot>().owner = gameObject;
			genericSlotList.Add(s.GetComponent<ItemSlot>());
		}

		for ( int i=0; i<armorSlots; i++ ){
			GameObject s = Instantiate(temp, Vector3.zero, Quaternion.identity) as GameObject;
			s.GetComponent<ItemSlot>().itemSlotType = (int)ItemSlot.ItemType.Armor;
			s.transform.SetParent(itemContainer.transform);
			s.GetComponent<ItemSlot>().owner = gameObject;
			genericSlotList.Add(s.GetComponent<ItemSlot>());
		}

		for ( int i=0; i<accessorySlots; i++ ){
			GameObject s = Instantiate(temp, Vector3.zero, Quaternion.identity) as GameObject;
			s.GetComponent<ItemSlot>().itemSlotType = (int)ItemSlot.ItemType.Accessory;
			s.transform.SetParent(itemContainer.transform);
			s.GetComponent<ItemSlot>().owner = gameObject;
			genericSlotList.Add(s.GetComponent<ItemSlot>());
		}

		for ( int i=0; i<techSlots; i++ ){
			GameObject s = Instantiate(temp, Vector3.zero, Quaternion.identity) as GameObject;
			s.GetComponent<ItemSlot>().itemSlotType = (int)ItemSlot.ItemType.Technology;
			s.transform.SetParent(itemContainer.transform);
			s.GetComponent<ItemSlot>().owner = gameObject;
			genericSlotList.Add(s.GetComponent<ItemSlot>());
		}

		for ( int i=0; i<genericSlots; i++ ){
			GameObject s = Instantiate(temp, Vector3.zero, Quaternion.identity) as GameObject;
			s.transform.SetParent(itemContainer.transform);
			s.GetComponent<ItemSlot>().owner = gameObject;
			genericSlotList.Add(s.GetComponent<ItemSlot>());
		}

	}
	
	// Update is called once per frame
	void Update () {

		if ( itemContainer.GetComponent<GridLayoutGroup>() == null ){
			equipPanel.SetActive(false);
			itemContainer.AddComponent<GridLayoutGroup>();
			itemContainer.GetComponent<GridLayoutGroup>().padding.left = 8;
			itemContainer.GetComponent<GridLayoutGroup>().padding.top = 8;
			itemContainer.GetComponent<GridLayoutGroup>().padding.right = 8;
			itemContainer.GetComponent<GridLayoutGroup>().padding.bottom = 8;
			itemContainer.GetComponent<GridLayoutGroup>().cellSize = new Vector2(32, 32);
			itemContainer.GetComponent<GridLayoutGroup>().spacing = new Vector2(8, 8);
		}

	}

	public ItemSlot getFirstAvailableSlotOfType(string itemType){

		ItemSlot iS = null;
		List<ItemSlot> haystack = null;

		switch (itemType.ToLower() ){
		case "generic":
			haystack = genericSlotList;
			break;
		default:
			haystack = null;
			break;
		}
		
		if ( haystack != null ){
			for ( int i=0; i<genericSlots; i++ ){
				if ( haystack[i].GetComponent<ItemSlot>().item == null ){
					iS = haystack[i].GetComponent<ItemSlot>();
					break;
				}
			}
		}
	
		return iS;

	}
	


}
