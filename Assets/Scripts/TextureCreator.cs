using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LibNoise.Generator;

public class TextureCreator : MonoBehaviour {

	[Range(2, 1024)]
	public int resolution = 64;
	[Range(1f, 3f)]
	public float fastPointSpeed = 2f;
	[Range(0.01f, 1f)]
	public float slowPointSpeed = 0.2f;
	[Range(0f, 1f)]
	public float threshold = 0.5f;
	private Texture2D texture;

	public GameObject Voxels;

	[SerializeField]
	Vector3 _displacement = 0.0001f * Vector3.one;

	public Perlin perlin = new Perlin();

	public MeshGenerator gen;	

	void Start(){
		if ( GameObject.Find("Voxels") == null ){
			Voxels = new GameObject("Voxels");
		}
	}

	private void Update () {
		if (transform.hasChanged) {
			transform.hasChanged = false;
			FillTexture();
			//MakeVoxels();
		}
	}

	private void OnEnable(){

		if ( GameObject.Find("Voxels") == null ){
			Voxels = new GameObject("Voxels");
		}

		perlin.OctaveCount = 4;
		perlin.Frequency = 0.1f;
		if (texture == null) {
			texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
			texture.name = "Procedural Texture";
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Trilinear;
			texture.anisoLevel = 9;
			GetComponent<MeshRenderer>().material.mainTexture = texture;
		}
		FillTexture();
		//MakeVoxels();
	}

	public float frequency = 1f;

	public void FillTexture(){

		if (texture.width != resolution) {
			texture.Resize(resolution, resolution);
		}

		// Vector3 point00 = new Vector3(-0.5f,-0.5f);
		// Vector3 point10 = new Vector3( 0.5f,-0.5f);
		// Vector3 point01 = new Vector3(-0.5f, 0.5f);
		// Vector3 point11 = new Vector3( 0.5f, 0.5f);
		Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f,-0.5f));
		Vector3 point10 = transform.TransformPoint(new Vector3( 0.5f,-0.5f));
		Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
		Vector3 point11 = transform.TransformPoint(new Vector3( 0.5f, 0.5f));

		float stepSize = 1f / resolution;
		for (int y = 0; y < resolution; y++) {
			Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
			Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
			for (int x = 0; x < resolution; x++) {
				Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
				if ( valueAtPoint(point, frequency) > threshold )
					texture.SetPixel(x, y, Color.green);
				else 
					texture.SetPixel(x, y, Color.black);
			}
		}
		texture.Apply();
	}

	public void MakeVoxels(){

		if ( GameObject.Find("Voxels") == null ){
			Voxels = new GameObject("Voxels");
		} else {
			foreach( Transform child in Voxels.transform ){
				Destroy(child.gameObject);
			}
		}

		Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f,-0.5f));
		Vector3 point10 = transform.TransformPoint(new Vector3( 0.5f,-0.5f));
		Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
		Vector3 point11 = transform.TransformPoint(new Vector3( 0.5f, 0.5f));

		float stepSize = 1f / resolution;
		for (int y = 0; y < resolution; y++) {
			Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
			Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
			for (int x = 0; x < resolution; x++) {
				Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
				if ( valueAtPoint(point, frequency) > threshold ){
					GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
					//temp.transform.localScale = new Vector3(stepSize, stepSize, stepSize);
					temp.transform.position = new Vector3(point.x, point.y, point.z);
					temp.transform.SetParent(GameObject.Find("Voxels").transform);
					temp.renderer.material.color = Color.red;
				}
			}
		}
	}

	public void renderMeshWithNoise(){

		if ( GameObject.Find("Voxels") == null ){
			Voxels = new GameObject("Voxels");
		} else {
			foreach( Transform child in Voxels.transform ){
				Destroy(child.gameObject);
			}
		}

		gen = new MeshGenerator(GameObject.Find("Voxels").transform);
		gen.setMeshScale(1f);
		gen.setMeshPosition(new Vector3(0f, 0f, 0f));
		gen.setMeshSize (200,200);

		GameObject theMesh = gen.generateMeshFromValues(this.GetComponent<TextureCreator>());
		theMesh.name = "theMesh";
		theMesh.transform.SetParent(GameObject.Find("Voxels").transform);

	}

	public float valueAtPoint(Vector3 point, float frequency, int octaves = 1){
		float value = 0f;
		Vector3 pure = point;
		Vector3 fastPoint = point * fastPointSpeed;
		Vector3 slowPoint = point * slowPointSpeed;
		point *= frequency;

		// BASE SURFACE
		value = Noise.HorizontalGradient1D(point, 3f, 0f);

		//value += Noise.HorizontalGradient1D(point, -1.5f, -3f) * .25f;

		//value += Random.Range(-.25f, .25f);
		// value += (Mathf.PerlinNoise(slowPoint.x, slowPoint.y)-.5f) *.5f;
		// value += (Mathf.PerlinNoise(point.x, point.y)-.5f) *.5f;			
		// value += (Mathf.PerlinNoise(fastPoint.x, fastPoint.y)-.5f) *.5f;

		for (int j=0; j<octaves; j++){
			value += ((float)perlin.GetValue(point * Mathf.Pow(frequency, -j) + _displacement) - 0.25f ) * .5f;
		}

		//value += ((float)perlin.GetValue(point + _displacement) - 0.5f ) * .5f;

		return value;
	}



}
