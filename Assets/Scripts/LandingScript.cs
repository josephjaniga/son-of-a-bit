using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LandingScript : MonoBehaviour {
	
	public PlanetaryBody pb;
	public bool landing = true;

	public GameObject sct;

	// Use this for initialization
	void Start () {
	
		// the text box
		sct = Resources.Load("Prefabs/UI/SCT") as GameObject;
		sct.GetComponent<SCT>().isEnabled = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision hit){
		if ( hit.gameObject.name == "PlayerShip" || hit.gameObject.name == "Player" ){

			if ( landing ){
				// load this in planet to land
				GameObject.Find("FatherBit").GetComponent<Main>().planetToLand = pb.theBody;
				GameObject.Find("FatherBit").GetComponent<Main>().body = pb;
				GameObject.Find("FatherBit").GetComponent<Main>().returnToOrbit = false;
				
				// clear all the alerts
				foreach (Transform child in GameObject.Find("Alerts").transform) {
					GameObject.Destroy(child.gameObject);
				}

				if ( sct == null ){
					sct = Resources.Load("Prefabs/UI/SCT") as GameObject;
					sct.GetComponent<SCT>().isEnabled = false;
				}

				if ( sct != null ) {
					// show the message
					GameObject alert = Instantiate(sct, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
					alert.GetComponent<Text>().text = "Press [<color=#00DD00>Space</color>]: to Land on " + name;
					alert.transform.SetParent(GameObject.Find("Alerts").transform);
					alert.transform.localPosition = new Vector3(0f, -80f, 0f);
					alert.GetComponent<Text>().fontSize = 12;
					alert.GetComponent<SCT>().Timer = 6;
					alert.GetComponent<SCT>().Timeout = 6;
				}
			}

			if ( !landing ){

				// clear planet to land
				GameObject.Find("FatherBit").GetComponent<Main>().planetToLand = null;
				GameObject.Find("FatherBit").GetComponent<Main>().body = null;
				GameObject.Find("FatherBit").GetComponent<Main>().returnToOrbit = true;

				// clear all the alerts
				foreach (Transform child in GameObject.Find("Alerts").transform) {
					GameObject.Destroy(child.gameObject);
				}
				
				// show the message
				GameObject alert = Instantiate(sct, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
				alert.GetComponent<Text>().text = "Press [<color=#00DD00>Space</color>]: to return to orbit";
				alert.transform.SetParent(GameObject.Find("Alerts").transform);
				alert.transform.localPosition = new Vector3(0f, -80f, 0f);
				alert.GetComponent<Text>().fontSize = 12;
				alert.GetComponent<SCT>().Timer = 6;
				alert.GetComponent<SCT>().Timeout = 6;

			}

			
		}
		
	}

}
