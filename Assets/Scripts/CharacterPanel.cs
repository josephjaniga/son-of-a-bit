using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterPanel : MonoBehaviour {

	private GameObject player;
	public Bit bit;

	public GameObject HealthValue;
	public GameObject HealthRegenValue;
	public GameObject SpeedValue;
	public GameObject WeaponNameValue;
	public GameObject ROFValue;
	public GameObject NumberProjectilesValue;
	public GameObject ProjectileDamageValue;
	public GameObject DPSValue;



	// Use this for initialization
	void Start () {

		player = GameObject.Find("Player");
		bit = player.GetComponent<Bit>();

		HealthValue 			= GameObject.Find("HealthValue");
		HealthRegenValue 		= GameObject.Find("HealthRegenValue");
		SpeedValue 				= GameObject.Find("SpeedValue");
		WeaponNameValue 		= GameObject.Find("NameValue");
		ROFValue 				= GameObject.Find("RateOfFireValue");
		NumberProjectilesValue 	= GameObject.Find("NumberProjectilesValue");
		ProjectileDamageValue 	= GameObject.Find("ProjectileDamageValue");
		DPSValue 				= GameObject.Find("DPSValue");

	}
	
	// Update is called once per frame
	void Update () {

		Projectile p = bit.weapon.bullet.GetComponent<Projectile>();

		HealthValue.GetComponent<Text>().text 				= "" + bit.health.currentHP + " / " + bit.health.maxHP;
		HealthRegenValue.GetComponent<Text>().text 			= "+0hp / 5seconds";
		SpeedValue.GetComponent<Text>().text 				= "" + bit.motion.speed;
		WeaponNameValue.GetComponent<Text>().text 			= "" + p.projectileName;
		ROFValue.GetComponent<Text>().text 					= "" + (1.0f/p.ROF).ToString("f1") + " rounds / second";
		NumberProjectilesValue.GetComponent<Text>().text 	= "" + p.numProjectiles;
		ProjectileDamageValue.GetComponent<Text>().text 	= "" + p.projectileDamage;
		DPSValue.GetComponent<Text>().text 					= "" + (p.numProjectiles*p.projectileDamage/p.ROF).ToString("f1");

	}

}
