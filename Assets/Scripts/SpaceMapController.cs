using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SpaceMapController : MonoBehaviour {
	
	private GameObject origin;
	private List<GameObject> system;

	public GameObject systemContainer;

	public int maxSystemSize = 0;
	private int currentSystemSize = 0;

	// Use this for initialization
	void Start () {

		systemContainer = new GameObject();
		systemContainer.name = "System";

		system = new List<GameObject>();

		maxSystemSize = Random.Range (2, 11);

		origin = new GameObject();
		origin.name = "Origin";
		origin.AddComponent<PlanetaryBody>().recalculate();

		system.Add(origin);

		origin.transform.SetParent(systemContainer.transform);

		for ( currentSystemSize = 1; currentSystemSize < maxSystemSize; currentSystemSize++ ){
			GameObject newPlanetaryBody = new GameObject();
			newPlanetaryBody.name = "PB-" + currentSystemSize;
			newPlanetaryBody.AddComponent<PlanetaryBody>().isOrigin = false;
			newPlanetaryBody.GetComponent<PlanetaryBody>().index = currentSystemSize;
			newPlanetaryBody.GetComponent<PlanetaryBody>().recalculate();
			newPlanetaryBody.transform.SetParent(systemContainer.transform);
			system.Add(newPlanetaryBody);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
		foreach( Transform child in systemContainer.transform ){
			float revSpeed = child.GetComponent<PlanetaryBody>().revolutionSpeed;
			child.transform.RotateAround(Vector3.zero, Vector3.forward,  revSpeed * Time.deltaTime);
		}

	}


}
