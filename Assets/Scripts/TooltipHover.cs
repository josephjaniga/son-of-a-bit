using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class TooltipHover :  UIBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Container container;
	public GameObject TT;
	public GameObject TTStats;
	public GameObject TTName;
	public GameObject TTValue;
	public GameObject TTType;

	void Awake(){

		container = gameObject.GetComponent<Container>();
		TT = GameObject.Find ("TooltipPanel");
		TTStats = GameObject.Find ("TTStats");
		TTName = GameObject.Find ("TTName");
		TTValue = GameObject.Find ("TTValue");
		TTType = GameObject.Find ("TTType");

	}

	void Update(){
		TT = GameObject.Find("TooltipPanel");
	}
	
	// Use this for initialization
	void Start () {
	
		TT.SetActive(false);

	}

	public virtual void OnPointerEnter(PointerEventData eventData){

		TT.SetActive(true);
		
		TTName.GetComponent<Text>().text 	= container.item.itemName;
		TTValue.GetComponent<Text>().text 	= ""+container.item.value;
		
		switch (container.item.type){
			case 0:
			default:
				TTType.GetComponent<Text>().text = "Thing";
			break;
		}
		
		TTStats.GetComponent<Text>().text 	= container.item.statText;

	}

	public virtual void OnPointerExit(PointerEventData eventData){

		TT.SetActive(false);

	}

}
