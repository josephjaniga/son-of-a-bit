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

	// Use this for initialization
	void Start () {

		systemContainer = new GameObject();
		systemContainer.name = "System";

		system = new List<GameObject>();

		maxSystemSize = Random.Range (2, 9);

		origin = new GameObject();
		origin.name = "Origin";
		origin.AddComponent<PlanetaryBody>().recalculate();

		system.Add(origin);

		origin.transform.SetParent(systemContainer.transform);

		for ( currentSystemSize = 1; currentSystemSize < maxSystemSize; currentSystemSize++ ){

			GameObject newPlanetaryBody = new GameObject();
			newPlanetaryBody.name = "PB-" + currentSystemSize;
			newPlanetaryBody.AddComponent<PlanetaryBody>();
			newPlanetaryBody.GetComponent<PlanetaryBody>().name = newPlanetaryBody.name;
			newPlanetaryBody.GetComponent<PlanetaryBody>().index = currentSystemSize;
			newPlanetaryBody.GetComponent<PlanetaryBody>().recalculate(origin);
			newPlanetaryBody.transform.SetParent(systemContainer.transform);
			system.Add(newPlanetaryBody);

			// THATS NO MOON!
			if ( Random.Range (0f, 1f) < 5f ){ 

				GameObject newMoon = new GameObject();
				newMoon.name = "PB-" + currentSystemSize + "-A";
				newMoon.AddComponent<PlanetaryBody>();
				newMoon.GetComponent<PlanetaryBody>().isMoon = true;
				newMoon.GetComponent<PlanetaryBody>().name = newMoon.name;
				newMoon.GetComponent<PlanetaryBody>().index = newPlanetaryBody.GetComponent<PlanetaryBody>().index;
				newMoon.GetComponent<PlanetaryBody>().recalculate(newPlanetaryBody);
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
