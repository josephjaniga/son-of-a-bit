using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class PlanetaryBody : MonoBehaviour {

	public bool isOrigin = true;
	public GameObject orbitalParent = null;

	// planetary body attributes
	public string name = "LV-426";
	public float distanceFromParent = 0f;
	public float revolutionSpeed = 0f;
	public Vector3 rotation = Vector3.zero;
	public float sizeOfBody = 1f;

	public GameObject orbit = null;
	public int lengthOfLineRenderer = 90;

	public int index = 0;

	public GameObject theBody;

	public float startingRotation;
	
	public bool isMoon = false;

	public List<GameObject> children;

	// Use this for initialization
	void Start () {
		startingRotation = Random.Range(-15f, 15f);
	}
	
	// Update is called once per frame
	void Update () {
	
		// the planetary rotation
		Vector3 temp = rotation;
		temp.x *= Time.deltaTime;
		temp.y *= Time.deltaTime;
		temp.z *= Time.deltaTime;
		theBody.transform.Rotate(temp);


		foreach ( GameObject child in children ){
			child.transform.RotateAround(
				theBody.transform.position,
				Vector3.right, // x axis
				-temp.x
			);
			child.transform.RotateAround(
				theBody.transform.position,
				Vector3.up, // y axis
				-temp.y
			);
			child.transform.RotateAround(
				theBody.transform.position,
				Vector3.forward, // z axis
				-temp.z
			);
		}

		if ( !isOrigin ){
			setOrbit();
		}

	}
	
	public void recalculate(GameObject inheritedParent = null){

		children = new List<GameObject>();
		
		if ( inheritedParent != null ){
			isOrigin = false;
			orbitalParent = inheritedParent.GetComponent<PlanetaryBody>().theBody;
		}

		if ( theBody != null){
			Destroy (theBody.gameObject);
		}

		rotation = new Vector3(Random.Range(4f, 8f), Random.Range(4f, 8f), Random.Range(4f, 8f));

		if ( Random.Range (0f, 1f) < 0.5f ){
			rotation.x *= -1f;
		}
		if ( Random.Range (0f, 1f) < 0.5f ){
			rotation.y *= -1f;
		}
		if ( Random.Range (0f, 1f) < 0.5f ){
			rotation.z *= -1f;
		}
		
		// create a body with these properties
		theBody = null;
		theBody = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		theBody.name = name+"*";
		if ( isMoon ){
			if(orbitalParent != null) {
				theBody.transform.SetParent(
					orbitalParent.transform.parent.GetComponent<PlanetaryBody>().theBody.transform
				);
			}
		} else {
			theBody.transform.SetParent(gameObject.transform);
		}
		theBody.AddComponent<Rigidbody>();
		theBody.AddComponent<SphereCollider>();
		
		theBody.GetComponent<Rigidbody>().isKinematic = true;
		theBody.GetComponent<Rigidbody>().useGravity = false;
		
		if ( isOrigin ){
			theBody.transform.position = Vector3.zero;
			theBody.name = "PB"+"*";
			GameObject orbitContainer = new GameObject();
			orbitContainer.name = "Orbits";
			orbitContainer.transform.SetParent(gameObject.transform);
			sizeOfBody = Random.Range(15f, 22f);
		} else {

			// set distance position & size
			if ( isMoon ){
				float parentSize = orbitalParent.transform.parent.GetComponent<PlanetaryBody>().sizeOfBody;
				sizeOfBody = Random.Range(0.1f, 0.4f);
				distanceFromParent = Random.Range(parentSize, parentSize*2f) + parentSize/2f;
			} else {
				sizeOfBody = Random.Range(4f, 8f);
				if ( index < 3 ){
					sizeOfBody *= Random.Range(0.25f, 1.5f);
				}
				if ( index > 5 && index < 9 ){
					sizeOfBody *= Random.Range(8f, 11f);
				}
				distanceFromParent = Random.Range(sizeOfBody*3f+15f*index, sizeOfBody*11f+15f*index);
			}
			

			theBody.transform.position = new Vector3(orbitalParent.transform.position.x, orbitalParent.transform.position.y + distanceFromParent * index, orbitalParent.transform.position.z);

			revolutionSpeed = Random.Range(1f,5f);
			if ( Random.Range (0f, 1f) < 0.5f ){ // clockwise or counterclockwise
				revolutionSpeed *= -1f;
			}

			//transform.RotateAround(orbitalParent.transform.position, orbitalParent.transform.forward, Random.Range(0f, 360f) * startingRotation);

		}

		theBody.transform.localScale = new Vector3( sizeOfBody, sizeOfBody, sizeOfBody );

		if ( !isOrigin ){
			orbit = new GameObject();
			orbit.name = name + "(Orbital Path)";
			orbit.transform.position = orbitalParent.transform.position;

			orbit.AddComponent<LineRenderer>();
			LineRenderer lr = orbit.GetComponent<LineRenderer>();
			lr.material = new Material(Shader.Find("Particles/Additive"));
			lr.useWorldSpace  = true;
			lr.SetWidth(0.5f,0.5f);
			lr.SetColors(Color.green, new Color(0f, 0f, 1f, 0.25f));
			lr.SetVertexCount(lengthOfLineRenderer+1);

			if ( isMoon ){
				lr.SetWidth(0.25f,0.25f);
				theBody.renderer.material.color = Color.red;
				lr.SetColors(Color.yellow, Color.white);
			}

			setOrbit();

			orbit.transform.SetParent(GameObject.Find ("Origin").transform.FindChild("Orbits").transform);
			orbit.transform.RotateAround(orbitalParent.transform.position, orbitalParent.transform.forward, revolutionSpeed * startingRotation);
		}

		if ( isMoon && orbitalParent != null ){
			orbitalParent.transform.parent.GetComponent<PlanetaryBody>().children.Add(theBody);
		}
		
	}


	public void setOrbit(){

		LineRenderer lr = orbit.GetComponent<LineRenderer>();

		float deltaTheta = (2.0f * Mathf.PI) / lengthOfLineRenderer;
		float theta = 0f;
		float radius = distanceFromParent * index;

		for(int i = 0; i < lengthOfLineRenderer+1; i++){
			float x = radius * Mathf.Cos(theta);
			float y = radius * Mathf.Sin(theta);
			Vector3 pos = new Vector3(
				x + orbitalParent.transform.position.x,
				y + orbitalParent.transform.position.y,
				0f
			);
			lr.SetPosition(i, pos);
			theta += deltaTheta;
		}

	}
	
	
	
}
