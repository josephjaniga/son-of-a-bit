using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum PlanetTypes {
	EarthLike,
	IcePlanet,
	Moon,
	Cube
};

public class PlanetaryBody : MonoBehaviour {

	public PlanetTypes planetType = PlanetTypes.EarthLike;

	public bool isOrigin = true;
	public GameObject orbitalParent = null;

	// planetary body attributes
	public string thePlanetName = "";
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
	public bool isGiant = false;

	public List<GameObject> children;

	public int levelSeed = -1;
	public LevelTypes levelType = LevelTypes.Surface;
	public List<Material> materials = new List<Material>();

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

		Random.seed = levelSeed;
		planetType = GetRandomEnum<PlanetTypes>();

		children = new List<GameObject>();
		
		if ( inheritedParent != null ){
			isOrigin = false;
			orbitalParent = inheritedParent.GetComponent<PlanetaryBody>().theBody;
		}

		if ( theBody != null ){
			Destroy (theBody.gameObject);
		}

		setRotation ();
		
		// create a body with these properties
		theBody = null;

		DataProvider dp = GameObject.Find("DataProvider").GetComponent<DataProvider>();

		planetType = PlanetTypes.EarthLike;

		switch (planetType){
			default:
			case PlanetTypes.Moon:
				theBody = Instantiate(Resources.Load("Prefabs/Planets/MoonLike"), Vector3.zero, Quaternion.identity) as GameObject;
			break;
			case PlanetTypes.EarthLike:
				theBody = Instantiate(Resources.Load("Prefabs/Planets/EarthLike"), Vector3.zero, Quaternion.identity) as GameObject;
			break;
			case PlanetTypes.IcePlanet:
				theBody = Instantiate(Resources.Load("Prefabs/Planets/StrangeIce"), Vector3.zero, Quaternion.identity) as GameObject;
			break;
			case PlanetTypes.Cube:
				theBody = GameObject.CreatePrimitive(PrimitiveType.Cube);
				theBody.renderer.material.color = new Color(1f, 0f, 1f);
				levelType = LevelTypes.Rooms;
				dp.minRooms = Random.Range(1, 15);
				dp.maxRooms = dp.minRooms * Random.Range(1, 3);
			break;
		}

		// if ( isOrigin ){
		// 	theBody = Instantiate(Resources.Load("Prefabs/Planets/IceRockSand_2"), Vector3.zero, Quaternion.identity) as GameObject;
		// } else if ( isMoon ){
		// 	theBody = Instantiate(Resources.Load("Prefabs/Planets/MoonLike_2"), Vector3.zero, Quaternion.identity) as GameObject;
		// } else {
		// 	theBody = Instantiate(Resources.Load("Prefabs/Planets/EarthLike_2"), Vector3.zero, Quaternion.identity) as GameObject;
		// }

		theBody.AddComponent<LandingScript>().pb = this;

		theBody.name = thePlanetName+"*";

		if ( isMoon ){
			if(orbitalParent != null) {
				theBody.transform.SetParent(
					orbitalParent.transform.parent.GetComponent<PlanetaryBody>().theBody.transform
				);
			}
		} else {
			theBody.transform.SetParent(gameObject.transform);
		}


		if ( isOrigin ){
			theBody.transform.position = Vector3.zero;
			theBody.name = thePlanetName;
			GameObject orbitContainer = new GameObject();
			orbitContainer.name = "Orbits";
			orbitContainer.transform.SetParent(gameObject.transform);
			sizeOfBody = Random.Range(30f, 40f);
		} else {

			// set distance position & size
			if ( isMoon ){
				float parentSize = orbitalParent.transform.parent.GetComponent<PlanetaryBody>().sizeOfBody;
				sizeOfBody = Random.Range(0.5f, 0.8f);
				distanceFromParent = Random.Range(parentSize, parentSize*2f) + parentSize/2f;
			} else {

				sizeOfBody = Random.Range(30f, 40f);

				// Giants
				if ( index > 2 ){
					if ( Random.Range (0f,1f) < 1f ){
						isGiant = true;
						sizeOfBody *= 10f;
					}
				}

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
		}

		theBody.transform.localScale = new Vector3( sizeOfBody, sizeOfBody, sizeOfBody );

		if ( !isOrigin ){
			orbit = new GameObject();
			orbit.name = thePlanetName + "(Orbital Path)";
			orbit.transform.position = orbitalParent.transform.position;

			orbit.AddComponent<LineRenderer>();
			LineRenderer lr = orbit.GetComponent<LineRenderer>();
			lr.material = new Material(Shader.Find("Particles/Additive"));
			lr.useWorldSpace  = true;
			lr.SetWidth(0.75f,0.75f);
			lr.SetColors(new Color(0f,1f,0f,0.125f), new Color(0f,1f,0f,0.125f));
			lr.SetVertexCount(lengthOfLineRenderer+1);

			if ( isMoon ){
				lr.SetWidth(0.25f,0.25f);
				//theBody.renderer.material.color = Color.red;
				lr.SetColors(new Color(1f,1f,0f,0.125f), new Color(1f,1f,0f,0.125f));
			}

			setOrbit();

			orbit.transform.SetParent(GameObject.Find ("Origin").transform.FindChild("Orbits").transform);
		}

		if ( isMoon && orbitalParent != null ){
			orbitalParent.transform.parent.GetComponent<PlanetaryBody>().children.Add(theBody);
		}

		foreach ( Material m in theBody.renderer.materials ){
			materials.Add(m);
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

			pos = RotatePointAroundPivot(
				pos,
				orbitalParent.transform.parent.GetComponent<PlanetaryBody>().theBody.transform.position,
				Quaternion.Euler(0f, 0f, theBody.transform.rotation.z)
			);
			
			lr.SetPosition(i, pos);
			theta += deltaTheta;
		}

	}


	public void setRotation(){

		// set the rotation
		int numberAxesToRotate = Random.Range (0, 3);
		
		float rotationX = 0f;
		float rotationY = 0f;
		float rotationZ = 0f;
		
		do {
			int i = Random.Range (0,3);
			
			switch(i){
				case 0:
					//Debug.Log("X");
					rotationX = Random.Range(4f, 8f);
					break;
				default:
				case 1:
					//Debug.Log("Y");
					rotationY = Random.Range(4f, 8f);
					break;
				case 2:
					//Debug.Log("Z");
					rotationZ = Random.Range(4f, 8f);
					break;
			}
			
			numberAxesToRotate--;
			
		} while ( numberAxesToRotate > 0 );
		
		rotation = new Vector3(rotationX, rotationY, rotationZ);
		
		if ( Random.Range (0f, 1f) < 0.5f ){
			rotation.x *= -1f;
		}
		if ( Random.Range (0f, 1f) < 0.5f ){
			rotation.y *= -1f;
		}
		if ( Random.Range (0f, 1f) < 0.5f ){
			rotation.z *= -1f;
		}

	}

	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle) {
		return angle * ( point - pivot ) + pivot;
	}

    static T GetRandomEnum<T>(){
	    System.Array A = System.Enum.GetValues(typeof(T));
	    T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
	    return V;
    }
	
	
}
