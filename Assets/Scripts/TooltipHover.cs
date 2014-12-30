using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class TooltipHover :  UIBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public ItemSlot container;
	public GameObject TT;
	public CanvasGroup CG;
	public GameObject TTStats;
	public GameObject TTName;
	public GameObject TTValue;
	public GameObject TTType;
	public GameObject TTIcon;

	void Awake(){

		container = gameObject.GetComponent<ItemSlot>();
		TT = GameObject.Find ("TooltipPanel");
		CG = TT.GetComponent<CanvasGroup>();
		TTStats = GameObject.Find ("TTStats");
		TTName = GameObject.Find ("TTName");
		TTValue = GameObject.Find ("TTValue");
		TTType = GameObject.Find ("TTType");
		TTIcon = GameObject.Find ("TTIcon");
		hideToolTip();

	}

	// Use this for initialization
	void Start () {

		TT = GameObject.Find ("TooltipPanel");
		CG = TT.GetComponent<CanvasGroup>();
		hideToolTip();

	}

	
	void Update(){

	}

	public virtual void OnPointerEnter(PointerEventData eventData){

		if ( container.item != null ){

			showToolTip ();
			TTName.GetComponent<Text>().text 	= container.item.itemName;
			TTValue.GetComponent<Text>().text 	= ""+container.item.value;
			TTIcon.GetComponent<Image>().sprite = container.item.itemIcon; 

			switch (container.item.type){
			case (int)ItemSlot.ItemType.Technology:
				TTType.GetComponent<Text>().text = "Technology";
				break;
			case (int)ItemSlot.ItemType.Accessory:
				TTType.GetComponent<Text>().text = "Accessory";
				break;
			case (int)ItemSlot.ItemType.Armor:
				TTType.GetComponent<Text>().text = "Armor";
				break;
			case (int)ItemSlot.ItemType.Weapon:
				TTType.GetComponent<Text>().text = "Weapon";
				break;
			case (int)ItemSlot.ItemType.Generic:
			default:
				TTType.GetComponent<Text>().text = "Generic";
				break;
			}


			
			TTStats.GetComponent<Text>().text 	= container.item.statText;
		}

	}

	public virtual void OnPointerExit(PointerEventData eventData){
		hideToolTip();
	}

	public void hideToolTip(){
		CG.alpha = 0f;
	}

	public void showToolTip(){
		CG.alpha = 1f;
	}

}
