using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public int itemId = 0;
	public string itemName = "Excaliber";
	public int value = 1;
	public bool isEquipped = false;

	public enum ItemType {
		Junk
	};
	
	public int type = (int)ItemType.Junk;

	// Stats
	public int maxHealthBoost 				= 0;
	public float maxHealthScale 			= 0.0f;
	public float movementSpeedBoost 		= 0.0f;
	public float rateOfFireScale 			= 0.0f;
	public int numberOfProjectilesBoost 	= 0;
	public int projectileDamageBoost 		= 0;
	public float projectileDamageScale 		= 0.0f;
	public float critChanceBoost			= 0.0f;
	public float critDamageBoost			= 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void cloneItem(Item clone){
		itemId 						= clone.itemId;
		itemName					= clone.itemName;
		value		 				= clone.value;
		isEquipped					= clone.isEquipped;
		type						= clone.type;
		maxHealthBoost				= clone.maxHealthBoost;
		maxHealthScale				= clone.maxHealthScale;
		movementSpeedBoost			= clone.movementSpeedBoost;
		rateOfFireScale				= clone.rateOfFireScale;
		numberOfProjectilesBoost	= clone.numberOfProjectilesBoost;
		projectileDamageBoost		= clone.projectileDamageBoost;
		projectileDamageScale		= clone.projectileDamageScale;
		critChanceBoost				= clone.critChanceBoost;
		critDamageBoost				= clone.critDamageBoost;
	}


}
