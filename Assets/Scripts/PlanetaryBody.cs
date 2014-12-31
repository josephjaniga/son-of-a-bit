using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlanetaryBody : MonoBehaviour {

	public bool isOrigin = true;

	// planetary body attributes
	public string name = "LV-426";
	public float distanceFromOrigin = 0f;
	public float revolutionSpeed = 0f;
	public Vector3 rotation = Vector3.zero;
	public float sizeOfBody = 1f;

	public GameObject orbit = null;
	public int lengthOfLineRenderer = 360;

	public int index = 0;

	public GameObject theBody;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
		// the planetary rotation
		Vector3 temp = rotation;
		temp.x *= Time.deltaTime;
		temp.y *= Time.deltaTime;
		temp.z *= Time.deltaTime;
		theBody.transform.Rotate(temp);

		// the orbit path
//		if ( orbit != null ){
//			LineRenderer lr = orbit.GetComponent<LineRenderer>();
//			int i = 0;
//			while (i < lengthOfLineRenderer) {
//				Vector3 pos = new Vector3(i * 0.5F, Mathf.Sin(i + Time.time), 0);
//				lr.SetPosition(i, pos);
//				i++;
//			}
//		}
		
	}
	
	public void recalculate(){

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
		theBody = GameObject.CreatePrimitive(PrimitiveType.Cube);
		theBody.name = name;
		theBody.transform.SetParent(gameObject.transform);
		theBody.AddComponent<Rigidbody>();
		theBody.AddComponent<SphereCollider>();
		
		theBody.GetComponent<Rigidbody>().isKinematic = true;
		theBody.GetComponent<Rigidbody>().useGravity = false;
		
		if ( isOrigin ){
			theBody.transform.position = Vector3.zero;
			sizeOfBody = Random.Range(6f, 11f);
		} else {
			// set distance position & size
			sizeOfBody = Random.Range(3f, 6f);
			if ( index < 3 ){
				sizeOfBody *= Random.Range(0.25f, 0.6f);
			}
			if ( index > 5 && index < 9 ){
				sizeOfBody *= Random.Range(5f, 6f);
			}

			distanceFromOrigin = Random.Range(sizeOfBody*3f+1f, sizeOfBody*11f+1f);
			theBody.transform.position = new Vector3(0f, distanceFromOrigin * index, 0f);

			revolutionSpeed = Random.Range(5f,15f);
			if ( Random.Range (0f, 1f) < 0.5f ){
				revolutionSpeed *= -1f;
			}

			transform.RotateAround(Vector3.zero, Vector3.forward, revolutionSpeed * Random.Range(-15f, 15f));
		}
		
		theBody.transform.localScale = new Vector3( sizeOfBody, sizeOfBody, sizeOfBody );
		// theBody.rigidbody.angularVelocity = rotation;

		if ( !isOrigin ){
			orbit = new GameObject();
			orbit.name = "PB-" + index + "(Orbital Path)";

			orbit.AddComponent<LineRenderer>();
			LineRenderer lr = orbit.GetComponent<LineRenderer>();
			lr.material = new Material(Shader.Find("Particles/Additive"));
			lr.useWorldSpace  = true;
			lr.SetWidth(1f,1f);
			lr.SetColors(Color.green, Color.blue);
			lr.SetVertexCount(lengthOfLineRenderer+1);

			float deltaTheta = (2.0f * Mathf.PI) / lengthOfLineRenderer;
			float theta = 0f;
			float radius = distanceFromOrigin * index;

			for(int i = 0; i < lengthOfLineRenderer+1; i++){
				float x = radius * Mathf.Cos(theta);
				float y = radius * Mathf.Sin(theta);
				Vector3 pos = new Vector3(x, y, 0f);
				lr.SetPosition(i, pos);
				theta += deltaTheta;
			}

			orbit.transform.SetParent(GameObject.Find ("Origin").transform);
		}

	}




}
