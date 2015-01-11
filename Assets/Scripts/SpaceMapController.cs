using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SpaceMapController : MonoBehaviour {

	private GameObject origin;
	public List<GameObject> system;

	public GameObject systemContainer;

	public int maxSystemSize = 0;
	private int currentSystemSize = 0;

	public int seed;

	public Main m;
	public DataProvider dp;
	public GameObject ship;
	public GameObject playa;

	public string systemName = "";

	// Use this for initialization
	void Start () {

		generateSystem();

	}


	// Update is called once per frame
	void Update () {

		foreach( GameObject child in system ){
			PlanetaryBody pb = child.GetComponent<PlanetaryBody>();
			if ( !pb.isOrigin
			     && pb.theBody != null
			     && pb.orbitalParent != null
			     && pb.orbitalParent.transform.parent.GetComponent<PlanetaryBody>().theBody ){

				float revSpeed = pb.revolutionSpeed;
				pb.theBody.transform.position = RotatePointAroundPivot(
					pb.theBody.transform.position,
					pb.orbitalParent.transform.parent.GetComponent<PlanetaryBody>().theBody.transform.position,
					Quaternion.Euler(0, 0, revSpeed * Time.deltaTime)
				);
			}
		}

	}


	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle) {
		return angle * ( point - pivot ) + pivot;
	}

	[ContextMenu("Reset")] 
	public void Reset(){

		Destroy(GameObject.Find("System"));
		generateSystem();

	}

	public void generateSystem(){

		m = GameObject.Find ("FatherBit").GetComponent<Main>();
		dp = GameObject.Find ("DataProvider").GetComponent<DataProvider>();
		ship = GameObject.Find ("PlayerShip");
		playa = GameObject.Find ("Player");

		if ( dp.systemSeed == -1 ){
			seed = Random.Range(0,9999);
			dp.systemSeed = seed;
		} else {
			seed = dp.systemSeed;
		}

		if ( dp.playerSystemInShip && playa != null ){
			// put the player in the ship and set the ship as unit in control
			playa.GetComponent<Motion>().userControlled = false;
			ship.GetComponent<Motion>().userControlled = true;
			m.unitInControl = ship;
			ship.GetComponent<Vehicle>().seat = playa;
			m.inVehicle = true;
			m.v = ship.GetComponent<Vehicle>();
			playa.SetActive(false);
			Camera.main.GetComponent<MiniMapCameraFollow>().target = m.unitInControl.transform;
		}

		Debug.Log ("Random Seed: "+ seed);
		Random.seed = seed;

		systemName = TextTools.WordFinder2((int)Random.Range(3,6));

		systemContainer = new GameObject();
		systemContainer.name = "System";

		system = new List<GameObject>();

		maxSystemSize = Random.Range (2, 25);

		origin = new GameObject("Origin");
		PlanetaryBody origin_pb = origin.AddComponent<PlanetaryBody>();
		origin_pb.levelSeed = Random.Range(0,9999);
		origin_pb.thePlanetName = systemName + "-0*";
		origin_pb.recalculate();

		system.Add(origin);

		origin.transform.SetParent(systemContainer.transform);

		for ( currentSystemSize = 1; currentSystemSize < maxSystemSize; currentSystemSize++ ){

			GameObject newPlanetaryBody = new GameObject(systemName + "-" + currentSystemSize);
			PlanetaryBody pb = newPlanetaryBody.AddComponent<PlanetaryBody>();
			pb.thePlanetName = newPlanetaryBody.name;
			pb.index = currentSystemSize;
			pb.levelSeed = Random.Range(0,9999);
			pb.recalculate(origin);
			newPlanetaryBody.transform.SetParent(systemContainer.transform);
			system.Add(newPlanetaryBody);

			// THATS NO MOON!
			if ( Random.Range (0f, 1f) < 5f ){

				GameObject newMoon = new GameObject(systemName + "-" + currentSystemSize + "-A");
				PlanetaryBody moon_pb = newMoon.AddComponent<PlanetaryBody>();
				moon_pb.isMoon = true;
				moon_pb.thePlanetName = newMoon.name;
				moon_pb.index = newPlanetaryBody.GetComponent<PlanetaryBody>().index;
				moon_pb.levelSeed = Random.Range(0,9999);
				moon_pb.recalculate(newPlanetaryBody);
				newMoon.transform.SetParent(systemContainer.transform);
				system.Add(newMoon);

			}

		}


		foreach( GameObject child in system ){
			PlanetaryBody pb = child.GetComponent<PlanetaryBody>();
			if ( !pb.isOrigin
			    && pb.theBody != null
			    && pb.orbitalParent != null
			    && pb.orbitalParent.transform.parent.GetComponent<PlanetaryBody>().theBody ){

				float revSpeed = pb.revolutionSpeed;
				pb.theBody.transform.position = RotatePointAroundPivot(
					pb.theBody.transform.position,
					pb.orbitalParent.transform.parent.GetComponent<PlanetaryBody>().theBody.transform.position,
					Quaternion.Euler(0, 0, revSpeed * Random.Range (-360f, 360f) )
				);
			}
		}

		if ( dp.playerSystemLastPosition != Vector3.zero && GameObject.Find(dp.lastPlanetName) != null ) {
			m.unitInControl.transform.position = GameObject.Find(dp.lastPlanetName).transform.position - dp.playerSystemLastPosition;
		}

		TextTools.clearAlerts();
		TextTools.createAlert("- The " + systemName + " System -");
	}


}
