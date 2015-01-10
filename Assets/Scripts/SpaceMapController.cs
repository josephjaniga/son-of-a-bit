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
	
	// Use this for initialization
	void Start () {

		DataProvider dp = GameObject.Find ("DataProvider").GetComponent<DataProvider>();

		if ( dp.systemSeed == -1 ){
			seed = Random.Range(0,9999);
			dp.systemSeed = seed;
		} else {
			seed = dp.systemSeed;
		}

		Debug.Log ("Random Seed: "+ seed);
		Random.seed = seed;

		systemContainer = new GameObject();
		systemContainer.name = "System";

		system = new List<GameObject>();

		maxSystemSize = Random.Range (2, 25);

		origin = new GameObject();
		origin.name = "Origin";
		PlanetaryBody origin_pb = origin.AddComponent<PlanetaryBody>();
		origin_pb.levelSeed = Random.Range(0,9999);
		origin_pb.recalculate();

		system.Add(origin);

		origin.transform.SetParent(systemContainer.transform);

		for ( currentSystemSize = 1; currentSystemSize < maxSystemSize; currentSystemSize++ ){

			GameObject newPlanetaryBody = new GameObject("PB-" + currentSystemSize);
			PlanetaryBody pb = newPlanetaryBody.AddComponent<PlanetaryBody>();
			pb.name = newPlanetaryBody.name;
			pb.index = currentSystemSize;
			pb.levelSeed = Random.Range(0,9999);
			pb.recalculate(origin);
			newPlanetaryBody.transform.SetParent(systemContainer.transform);
			system.Add(newPlanetaryBody);

			// THATS NO MOON!
			if ( Random.Range (0f, 1f) < 5f ){ 

				GameObject newMoon = new GameObject("PB-" + currentSystemSize + "-A");
				PlanetaryBody moon_pb = newMoon.AddComponent<PlanetaryBody>();
				moon_pb.isMoon = true;
				moon_pb.name = newMoon.name;
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


}
