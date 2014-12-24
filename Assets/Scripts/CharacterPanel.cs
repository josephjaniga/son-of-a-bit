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
	public GameObject CriticalChanceValue;
	public GameObject CriticalDamageValue;
	public GameObject DPSValue;


	// hud panel
	public GameObject HUDWeaponNameValue;
	public GameObject HUDCredits;

	public GameObject GameMode;
	public GameObject WaveNumber;

	private Projectile unitsProjectile;

	void Awake(){
		player = GameObject.Find("Player");
		player = GameObject.Find("PlayerShip");
		if ( player != null){
			bit = player.GetComponent<Bit>();
		}
	}

	// Use this for initialization
	void Start () {

		player = GameObject.Find("Player");
		player = GameObject.Find("PlayerShip");
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
		CriticalChanceValue 	= GameObject.Find("CriticalChanceValue");
		CriticalDamageValue		= GameObject.Find("CriticalDamageValue");
		DPSValue 				= GameObject.Find("DPSValue");


		// HUD Panel
		if ( GameObject.Find("WeaponName") != null && GameObject.Find("CreditsValue") != null ){
			HUDWeaponNameValue 	= GameObject.Find("WeaponName");
			HUDCredits		 	= GameObject.Find("CreditsValue");
		}

		if ( GameObject.Find("GameModeValue") != null && GameObject.Find("WaveValue") != null ){
			GameMode = GameObject.Find("GameModeValue");
			WaveNumber = GameObject.Find("WaveValue");
		}

	}
	
	// Update is called once per frame
	void LateUpdate () {

		if ( bit != null && bit.weapon.bullet != null ){
			unitsProjectile = bit.weapon.bullet.GetComponent<Projectile>();
		}

		if ( player != null && bit != null  && bit.health != null){
			// These are flat values
			// Character Pane
			StatManager sm = bit.statManager;

			HealthValue.GetComponent<Text>().text 				= "" + bit.health.currentHP + " / " + sm.cMaxHealth;
			HealthRegenValue.GetComponent<Text>().text 			= "+" + sm.cRegen + " hp / " + bit.health.regenRate + " seconds";
			SpeedValue.GetComponent<Text>().text 				= "" + sm.cMovementSpeed;
			WeaponNameValue.GetComponent<Text>().text 			= "" + unitsProjectile.projectileName;
			ROFValue.GetComponent<Text>().text 					= "" + sm.cRateOfFire;
			NumberProjectilesValue.GetComponent<Text>().text 	= "" + sm.cNumberOfProjectiles;
			ProjectileDamageValue.GetComponent<Text>().text 	= "" + sm.cProjectileDamage;
			CriticalChanceValue.GetComponent<Text>().text 		= "" + sm.cCriticalChance * 100f + "%";
			CriticalDamageValue.GetComponent<Text>().text 		= "" + sm.cCriticalDamage * 100f + "%";;
			DPSValue.GetComponent<Text>().text 					= "" + (sm.cNumberOfProjectiles*sm.cProjectileDamage/sm.cRateOfFire).ToString("f1");

			// HUD Pane
			if ( HUDWeaponNameValue != null && HUDCredits != null ){
				HUDWeaponNameValue.GetComponent<Text>().text 	= "" + unitsProjectile.projectileName;
				HUDCredits.GetComponent<Text>().text 		 	= "" + bit.inventory.credits + " credits";
			}

			// Base Defense
			if ( GameMode != null && WaveNumber != null ){
				GameMode.GetComponent<Text>().text = "Protect Your Base!";
				WaveNumber.GetComponent<Text>().text = "Wave #" + GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>().waveNumber + "!";
			}
		}
	}

}
