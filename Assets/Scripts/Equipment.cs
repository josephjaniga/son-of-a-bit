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

	public List<ItemSlot> genericSlotList;

	// Use this for initialization
	void Start () {

		//make an Equipment panel
		GameObject canvas = GameObject.Find("Canvas");
		GameObject temp = Resources.Load("UI/Slot") as GameObject;
		GameObject panel = Resources.Load("UI/EquipmentPanel") as GameObject;
		equipPanel = Instantiate(panel, Vector3.zero, Quaternion.identity) as GameObject;
		equipPanel.transform.SetParent(canvas.transform);
		equipPanel.transform.localPosition = Vector3.zero;
		equipPanel.name = gameObject.name + "EquipmentPanel";

		// setup the slots
		genericSlotList = new List<ItemSlot>();

		// make the slots
		for ( int i=0; i<genericSlots; i++ ){
			GameObject s = Instantiate(temp, Vector3.zero, Quaternion.identity) as GameObject;
			s.transform.SetParent(equipPanel.transform);
			s.transform.localPosition = Vector3.zero;
			genericSlotList.Add(s.GetComponent<ItemSlot>());
		}

	}
	
	// Update is called once per frame
	void Update () {


	}


	public ItemSlot getFirstAvailableSlotOfType(string itemType){

		ItemSlot iS = null;
		List<ItemSlot> haystack = null;

		switch (itemType){
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
