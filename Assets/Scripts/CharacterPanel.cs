using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterPanel : MonoBehaviour {

	private GameObject player;
	public Bit bit;

	// character panel
	public GameObject HealthValue;
	public GameObject HealthRegenValue;
	public GameObject SpeedValue;
	public GameObject WeaponNameValue;
	public GameObject ROFValue;
	public GameObject NumberProjectilesValue;
	public GameObject ProjectileDamageValue;
	public GameObject DPSValue;

	// hud panel
	public GameObject HUDWeaponNameValue;
	public GameObject HUDCredits;

	private Projectile unitsProjectile;
	
	// Use this for initialization
	void Start () {

		player = GameObject.Find("Player");
		if ( player != null){
			bit = player.GetComponent<Bit>();
		}

		// Character Panel
		HealthValue 			= GameObject.Find("HealthValue");
		HealthRegenValue 		= GameObject.Find("HealthRegenValue");
		SpeedValue 				= GameObject.Find("SpeedValue");
		WeaponNameValue 		= GameObject.Find("NameValue");
		ROFValue 				= GameObject.Find("RateOfFireValue");
		NumberProjectilesValue 	= GameObject.Find("NumberProjectilesValue");
		ProjectileDamageValue 	= GameObject.Find("ProjectileDamageValue");
		DPSValue 				= GameObject.Find("DPSValue");

		// HUD Panel
		HUDWeaponNameValue 	= GameObject.Find("WeaponName");
		HUDCredits		 	= GameObject.Find("CreditsValue");
	}
	
	// Update is called once per frame
	void Update () {

		if ( bit != null && bit.weapon.bullet != null ){
			unitsProjectile = bit.weapon.bullet.GetComponent<Projectile>();
		}

		if ( player != null){
			// Character Pane
			HealthValue.GetComponent<Text>().text 				= "" + bit.health.currentHP + " / " + bit.health.maxHP;
			HealthRegenValue.GetComponent<Text>().text 			= "+0hp / 5seconds";
			SpeedValue.GetComponent<Text>().text 				= "" + bit.motion.speed;
			WeaponNameValue.GetComponent<Text>().text 			= "" + unitsProjectile.projectileName;
			ROFValue.GetComponent<Text>().text 					= "" + (1.0f/unitsProjectile.ROF).ToString("f1") + " rounds / second";
			NumberProjectilesValue.GetComponent<Text>().text 	= "" + unitsProjectile.numProjectiles;
			ProjectileDamageValue.GetComponent<Text>().text 	= "" + unitsProjectile.projectileDamage;
			DPSValue.GetComponent<Text>().text 					= "" + (unitsProjectile.numProjectiles*unitsProjectile.projectileDamage/unitsProjectile.ROF).ToString("f1");

			// HUD Pane
			HUDWeaponNameValue.GetComponent<Text>().text 	= "" + unitsProjectile.projectileName;;
			HUDCredits.GetComponent<Text>().text 		 	= "" + bit.inventory.credits + " credits";
		}
	}

}
