using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadOut : MonoBehaviour {

	public GameObject bullet;
	public GameObject rocket;

	public Toggle bullets;
	public Toggle rockets;

	public GameObject package;
	public PassThrough passThrough;

	// Use this for initialization
	void Start () {

		package = GameObject.Find ("Package");
		passThrough = package.GetComponent<PassThrough>();
		bullets = GameObject.Find ("BulletsToggle").GetComponent<Toggle>();
		rockets = GameObject.Find ("RocketsToggle").GetComponent<Toggle>();

	}
	
	// Update is called once per frame
	void Update () {
	
		if ( bullets.isOn == true ){
			passThrough.startingWeapon = bullet;
		} else if ( rockets.isOn == true) {
			passThrough.startingWeapon = rocket;
		}

	}

	public void begin(){

		// black out the screen
		foreach(Transform child in GameObject.Find("Panel").transform){
			child.gameObject.SetActive(false);
		}

		Application.LoadLevel("TowerDefense");

	}

}