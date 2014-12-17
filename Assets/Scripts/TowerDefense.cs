using UnityEngine;
using System.Collections;

public class TowerDefense : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
	
		if ( GameObject.Find("Player") == null ) {
			Application.LoadLevel("DeathScreen");
		}

	}


}
