using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public string itemName = "excaliber";
	public int value = 1;

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


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
