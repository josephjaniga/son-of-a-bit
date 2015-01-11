using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LandingScript : MonoBehaviour {
	
	public PlanetaryBody pb;
	public bool landing = true;

	// Use this for initialization
	void Start () {

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
				
				TextTools.clearAlerts();
				TextTools.createAlert("Press [<color=#00DD00>Space</color>]: to Land on " + name);
			}

			if ( !landing ){

				// clear planet to land
				GameObject.Find("FatherBit").GetComponent<Main>().planetToLand = null;
				GameObject.Find("FatherBit").GetComponent<Main>().body = null;
				GameObject.Find("FatherBit").GetComponent<Main>().returnToOrbit = true;

				TextTools.clearAlerts();
				TextTools.createAlert("Press [<color=#00DD00>Space</color>]: to return to orbit");

			}

			
		}
		
	}

}
