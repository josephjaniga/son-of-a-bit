using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Vehicle : MonoBehaviour {

	public string vehicleName = "Cube";
	public float cameraSize = 7;

	public GameObject sct;

	public GameObject seat = null;

	public Bit bit;

	// Use this for initialization
	void Start () {
	
		bit = gameObject.GetComponent<Bit>();
		
		// the text box
		sct = Resources.Load("Prefabs/UI/SCT") as GameObject;
		sct.GetComponent<SCT>().isEnabled = false;
		
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

			// clear all the alerts
			foreach (Transform child in GameObject.Find("Alerts").transform) {
				GameObject.Destroy(child.gameObject);
			}

			// show the message
			GameObject alert = Instantiate(sct, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
			alert.GetComponent<Text>().text = "Press [<color=#00DD00>E</color>]: to Board this " + vehicleName;
			alert.transform.SetParent(GameObject.Find("Alerts").transform);
			alert.transform.localPosition = new Vector3(0f, -80f, 0f);
			alert.GetComponent<Text>().fontSize = 12;
			alert.GetComponent<SCT>().Timer = 6;
			alert.GetComponent<SCT>().Timeout = 6;

		}

	}

	public void boardVehicle(GameObject player){
		seat = player;

		// clear all the alerts
		foreach (Transform child in GameObject.Find("Alerts").transform) {
			GameObject.Destroy(child.gameObject);
		}

		// show the message
		GameObject alert = Instantiate(sct, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
		alert.GetComponent<Text>().text = "Press [<color=#0000DD>Q</color>]: to Leave this " + vehicleName;
		alert.transform.SetParent(GameObject.Find("Alerts").transform);
		alert.transform.localPosition = new Vector3(0f, -80f, 0f);
		alert.GetComponent<Text>().fontSize = 12;
		alert.GetComponent<SCT>().Timer = 3;
		alert.GetComponent<SCT>().Timeout = 3;

	}
	
}
