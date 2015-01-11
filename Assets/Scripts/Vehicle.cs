using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Vehicle : MonoBehaviour {

	public string vehicleName = "Cube";
	public float cameraSize = 7;

	public GameObject seat = null;

	public Bit bit;

	// Use this for initialization
	void Start () {

		bit = gameObject.GetComponent<Bit>();

	}

	// Update is called once per frame
	void Update () {

		if ( bit.health != null && bit.health.isDead && seat != null ){
			seat.GetComponent<Bit>().health.isDead = true;
		}

		/**
		 * TODO: this VVV
		 */
		// kill and eject the dead player with some velocity and rotation
		if ( bit.health.isDead && seat != null ){

		}

	}

	void OnCollisionEnter(Collision hit){
		if ( hit.gameObject.name == "Player" ){
			// load this in the vehicle to board
			GameObject.Find("FatherBit").GetComponent<Main>().vehicleToBoard = gameObject;
			GameObject.Find("FatherBit").GetComponent<Main>().v = this;

			TextTools.clearAlerts();
			TextTools.createAlert("Press [<color=#00DD00>E</color>]: to Board this " + vehicleName);
		}
	}

	public void boardVehicle(GameObject player){
		seat = player;

		TextTools.clearAlerts();
		TextTools.createAlert("Press [<color=#0000DD>Q</color>]: to Leave this " + vehicleName);
	}

}
