using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public int itemId = 0;
	public string itemName = "Excaliber";
	public int value = 1;
	public bool isEquipped = false;

	public string statText;

	public Sprite itemIcon;

	public enum ItemRarity{
		Junk,
		Common,
		Uncommon,
		Rare,
		Epic,
		Legendary
	};
	
	public enum ItemType {
		Generic,
		Weapon,
		Armor,
		Accessory,
		Technology
	};

	public int rarity = (int)ItemRarity.Junk;
	public int type = (int)ItemType.Generic;

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
	public int healthRegenBoost				= 0;


	public int statCount = 10;
	public UnityEngine.Random r;
	
	// Use this for initialization
	void Start () {

		updateItemStatText();

	}
	
	// Update is called once per frame
	void Update () {

		if ( statText == "" ){
			updateItemStatText();
		}

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
		healthRegenBoost			= clone.healthRegenBoost;
	}

	public void randomize(int Rarity = 0){

		rarity = Rarity;
		
		int calculon = Rarity+1;
		int MaxNumStats = Rarity+1;

		itemIcon = Resources.LoadAll<Sprite>("Other/SciFi/scifi_items")[(int)Random.Range(0,197)];

		// choose stats
		for ( int i=0; i<MaxNumStats; i++ ){

			int statRoll = (int)Random.Range(1, statCount+1);
			int statValue = (int)Random.Range(1, 1+calculon/2.0f);
			float statPercent = Random.Range(1, 1+calculon) * 0.125f;
			
			value += statValue;
			
			itemName 					= WordFinder2((int)Random.Range(4,11));
			isEquipped					= false;
			type						= (int)Random.Range((int)ItemType.Generic, (int)ItemType.Technology+1);

			//Debug.Log ("i:" + i + " // rolled:" + statRoll);

			switch(statRoll){
			case (int)1:
				maxHealthBoost += statValue;
				break;
			case (int)2:
				maxHealthScale += statPercent * 0.5f;
				break;
			case (int)3:
				movementSpeedBoost += statPercent;
				break;
			case (int)4:
				rateOfFireScale += statPercent * 0.5f;
				break;
			case (int)5:
				//numberOfProjectilesBoost += statValue;
				break;
			case (int)6:
				projectileDamageBoost += statValue;
				break;
			case (int)7:
				projectileDamageScale += statPercent;
				break;
			case (int)8:
				critChanceBoost += statPercent * 0.1f;
				break;
			case (int)9:
				critDamageBoost += statPercent;
				break;
			case (int)10:
				healthRegenBoost += statValue;
				break;
			default:
				break;
			}

			
		}
		

	}
	

	public void askForId(){

		if ( itemId == 0 ){
			Inventory inv = GameObject.Find("FatherBit").GetComponent<Main>().getActiveInventory();
			inv.idCount++;
			itemId = inv.idCount;
		}

	}


	public void updateItemStatText(){
		statText = "";
		
		if ( maxHealthBoost != 0 ){
			statText += "+" + maxHealthBoost + " Maximum Health \r\n";
		}
		
		if ( maxHealthScale != 0f ){
			statText += "+" + maxHealthScale * 100f + "% Maximum Health \r\n";
		}
		
		if ( movementSpeedBoost != 0f ){
			statText += "+" + movementSpeedBoost * 100f + "% Movement Speed \r\n";
		}
		
		if ( rateOfFireScale != 0f ){
			statText += "+" + rateOfFireScale * 100f + "% Rate Of Fire \r\n";
		}
		
		if ( numberOfProjectilesBoost != 0 ){
			statText += "+" + numberOfProjectilesBoost + " Number Of Projectiles \r\n";
		}
		
		if ( projectileDamageBoost != 0 ){
			statText += "+" + projectileDamageBoost + " Projectile Damage \r\n";
		}
		
		if ( projectileDamageScale != 0f ){
			statText += "+" + projectileDamageScale * 100f + "% Projectile Damage \r\n";
		}
		
		if ( critChanceBoost != 0f ){
			statText += "+" + critChanceBoost * 100f + "% Critical Hit Chance \r\n";
		}
		
		if ( critDamageBoost != 0f ){
			statText += "+" + critDamageBoost * 100f + "% Critical Hit Damage \r\n";
		}

		if ( healthRegenBoost != 0 ){
			statText += "+" + healthRegenBoost + " health / 2 seconds \r\n";
		}

	}




	/* Random Name Crap */

	public string WordFinder2(int requestedLength){
		UnityEngine.Random rnd = new UnityEngine.Random();
		string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
		string[] vowels = { "a", "e", "i", "o", "u" };
		string word = "";
		if (requestedLength == 1){
			word = GetRandomLetter(rnd, vowels);
		} else {
			for (int i = 0; i < requestedLength; i+=2){
				word += GetRandomLetter(rnd, consonants) + GetRandomLetter(rnd, vowels);
			}
			word = word.Replace("q", "qu").Substring(0, requestedLength); // We may generate a string longer than requested length, but it doesn't matter if cut off the excess.
		}
		return char.ToUpper(word[0]) + word.Substring(1);
	}
	
	private static string GetRandomLetter(UnityEngine.Random rnd, string[] letters){
		return letters[Mathf.RoundToInt(UnityEngine.Random.Range(0, letters.Length - 1))];
	}
	
	
	
	
}
