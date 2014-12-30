using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TowerDefense : MonoBehaviour {

	public GameObject package;
	public GameObject player;

	public GameObject GameMode;
	public GameObject WaveNumber;

	// Use this for initialization
	void Start () {
	
		package = GameObject.Find("Package");
		player = GameObject.Find("Player");

		if ( package != null && player != null ){
			player.GetComponent<Weapon>().bullet = package.GetComponent<PassThrough>().startingWeapon;
			Destroy(package);
		}
		
		if ( GameObject.Find("GameModeValue") != null && GameObject.Find("WaveValue") != null ){
			GameMode = GameObject.Find("GameModeValue");
			WaveNumber = GameObject.Find("WaveValue");
		}

	}
	
	// Update is called once per frame
	void LateUpdate () {

		if ( player.GetComponent<Bit>().health != null && player.GetComponent<Bit>().health.isDead ) {
			Application.LoadLevel("DeathScreen");
		}

		// Base Defense
		if ( GameMode != null && WaveNumber != null ){
			GameMode.GetComponent<Text>().text = "Protect Your Base!";
			WaveNumber.GetComponent<Text>().text = "Wave #" + GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>().waveNumber + "!";
		}

	}


}
