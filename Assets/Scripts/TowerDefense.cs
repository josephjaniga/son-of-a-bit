using UnityEngine;
using System.Collections;

public class TowerDefense : MonoBehaviour {

	public GameObject package;
	public GameObject player;

	// Use this for initialization
	void Start () {
	
		package = GameObject.Find("Package");
		player = GameObject.Find("Player");

		if ( package != null && player != null ){
			player.GetComponent<Weapon>().bullet = package.GetComponent<PassThrough>().startingWeapon;
			Destroy(package);
		}

	}
	
	// Update is called once per frame
	void LateUpdate () {

		if ( player.GetComponent<Bit>().health != null && player.GetComponent<Bit>().health.isDead ) {
			Application.LoadLevel("DeathScreen");
		}

	}


}
