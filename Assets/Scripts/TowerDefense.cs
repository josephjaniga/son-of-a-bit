using UnityEngine;
using System.Collections;

public class TowerDefense : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		GameObject package = GameObject.Find("Package");
		GameObject player = GameObject.Find("Player");

		if ( package != null && player != null ){
			player.GetComponent<Weapon>().bullet = package.GetComponent<PassThrough>().startingWeapon;
			Destroy(package);
		}

	}
	
	// Update is called once per frame
	void LateUpdate () {
	
		if ( GameObject.Find("Player") == null ) {
			Application.LoadLevel("DeathScreen");
		}

	}


}
