using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LandingScript : MonoBehaviour {
	
	public PlanetaryBody pb;
	public bool landing = true;

	public DataProvider dp;

	public Main m;

	// Use this for initialization
	void Start () {
		m = GameObject.Find("FatherBit").GetComponent<Main>();
		dp = GameObject.Find("DataProvider").GetComponent<DataProvider>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision hit){
		if ( hit.gameObject.name == "PlayerShip" || hit.gameObject.name == "Player" ){

			if ( landing ){
				// load this in planet to land

				if ( m == null ){
					m = GameObject.Find("FatherBit").GetComponent<Main>();
				}
				m.planetToLand = pb.theBody;
				m.body = pb;
				m.returnToOrbit = false;

				if ( dp == null ){
					dp = GameObject.Find("DataProvider").GetComponent<DataProvider>();
				}

				dp.levelMaterials = pb.materials;
				dp.levelSeed = pb.levelSeed;
				dp.levelType = pb.levelType;
				dp.planetType = pb.planetType;

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
