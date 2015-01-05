﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LandingScript : MonoBehaviour {
	
	public PlanetaryBody pb;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision hit){
		if ( hit.gameObject.name == "PlayerShip" || hit.gameObject.name == "Player" ){
			
			// load this in planet to land
			GameObject.Find("FatherBit").GetComponent<Main>().planetToLand = pb.theBody;
			GameObject.Find("FatherBit").GetComponent<Main>().body = pb;
			
			// clear all the alerts
			foreach (Transform child in GameObject.Find("Alerts").transform) {
				GameObject.Destroy(child.gameObject);
			}
			
			// show the message
			GameObject alert = Instantiate(pb.sct, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
			alert.GetComponent<Text>().text = "Press [<color=#00DD00>E</color>]: to Land on " + name;
			alert.transform.SetParent(GameObject.Find("Alerts").transform);
			alert.transform.localPosition = new Vector3(0f, -80f, 0f);
			alert.GetComponent<Text>().fontSize = 12;
			alert.GetComponent<SCT>().Timer = 6;
			alert.GetComponent<SCT>().Timeout = 6;
			
		}
		
	}

}
