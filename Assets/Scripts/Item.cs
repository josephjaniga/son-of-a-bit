using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public int itemId = 0;
	public string itemName = "Excaliber";
	public int value = 1;
	public bool isEquipped = false;

	public enum ItemRarity{
		Junk
	};
	
	public enum ItemType {
		Thing
	};

	public int rarity = (int)ItemRarity.Junk;

	public int type = (int)ItemType.Thing;

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

	public void randomize(){

		itemName 					= WordFinder2(Mathf.RoundToInt(Random.Range(7,21)));
		value		 				= Mathf.RoundToInt(Random.Range(0,4));
		isEquipped					= false;
		type						= (int)ItemType.Thing;
		maxHealthBoost				= Mathf.RoundToInt(Random.Range(0,4));;
		maxHealthScale				= Random.Range(0,1);
		movementSpeedBoost			= Random.Range(0,1);
		rateOfFireScale				= Random.Range(0,1);
		numberOfProjectilesBoost	= Mathf.RoundToInt(Random.Range(0,4));
		projectileDamageBoost		= Mathf.RoundToInt(Random.Range(0,4));
		projectileDamageScale		= Random.Range(0,1);
		critChanceBoost				= Random.Range(0,1);
		critDamageBoost				= Random.Range(0,1);

	}

	public void askForId(){

		if ( itemId == 0 ){
			Inventory inv = GameObject.Find("Player").GetComponent<Inventory>();
			inv.idCount++;
			itemId = inv.idCount;
		}

	}





	/* Random Name Crap */

	public string WordFinder2(int requestedLength)
	{
		Random rnd = new Random();
		string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
		string[] vowels = { "a", "e", "i", "o", "u" };
		
		string word = "";
		
		if (requestedLength == 1)
		{
			word = GetRandomLetter(rnd, vowels);
		}
		else
		{
			for (int i = 0; i < requestedLength; i+=2)
			{
				word += GetRandomLetter(rnd, consonants) + GetRandomLetter(rnd, vowels);
			}
			
			word = word.Replace("q", "qu").Substring(0, requestedLength); // We may generate a string longer than requested length, but it doesn't matter if cut off the excess.
		}
		
		return word;
	}
	
	private static string GetRandomLetter(Random rnd, string[] letters)
	{
		return letters[Mathf.RoundToInt(Random.Range(0, letters.Length - 1))];
	}
	
	
	
	
}
